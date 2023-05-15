using Framework.Infrastructure.UOW;
using MediatR;
using MessageCore.Application.Services.Base;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using MessageCore.Domain.AggregatesModels.BlockingAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.IssuerAggregate;
using MessageCore.Domain.AggregatesModels.IssuerAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.OrderAggregate;
using MessageCore.Domain.AggregatesModels.OrderAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.OrderFilterAggregate;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using MessageCore.Domain.AggregatesModels.ThemeAggregate;
using MessageCore.Domain.AggregatesModels.ThemeAggregate.Specifications;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Repositories;
using MessageCore.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageCore.Domain.AggregatesModels.TemplateAggregate.Specifications;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Framework.Domain.Core.Entities;
using Framework.Infrastructure.Crosscutting;

namespace MessageCore.Application.OpenApi.Commands.OrderCommand.Handlers
{
    public class CreateOrderCommandHandler : AsyncRequestHandler<CreateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIssuerRepository _senderRepository;
        private readonly ITemplateRepository _templateRepository;

        private readonly IAttachmentService _attachmentService;
        private readonly ITemplateAnalysisService _templateAnalysisService;

        private readonly IThemeRepository _themeRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISendingServiceRepository _sendingServiceRepository;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly ISendingOrderRepository _sendingOrderRepository;
        private readonly IOrderFilterRepository _orderFilterRepository;
        private readonly IBlockingRepository _blockingRepository;

        private readonly ILogger<CreateOrderCommandHandler> _logger;

        private const string BlockingTemplateName = "*";//黑名单暂时用*，代表所有匹配


        public CreateOrderCommandHandler(IOrderRepository orderRepository, IIssuerRepository senderRepository,
            IAttachmentService attachmentService, ITemplateRepository templateRepository, IThemeRepository themeRepository,
            IUnitOfWork unitOfWork, ISendingServiceRepository sendingServiceRepository, ISubscriberRepository subscriberRepository,
            ISendingOrderRepository sendingOrderRepository, IOrderFilterRepository orderFilterRepository, IBlockingRepository blockingRepository,
            ITemplateAnalysisService templateAnalysisService, ILogger<CreateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _senderRepository = senderRepository;
            _templateRepository = templateRepository;

            _attachmentService = attachmentService;

            _themeRepository = themeRepository;

            _sendingServiceRepository = sendingServiceRepository;
            _subscriberRepository = subscriberRepository;
            _sendingOrderRepository = sendingOrderRepository;
            _orderFilterRepository = orderFilterRepository;
            _blockingRepository = blockingRepository;

            _templateAnalysisService = templateAnalysisService;

            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        protected override async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {           
            //验证模板
            var template = await _templateRepository.GetAsync(new MatchTemplateByNameSpecification<Template>(request.TemplateName));
            IsNotNull(template, "template:" + request.TemplateName);

            //模板没有开启
            if (!template.Enabled)
            {
                throw new MessageCoreInternalException("000000", "templateName is not enabled!" + request.TemplateName);
            }

            IsNotNullOrEmpty(template.IssuerIds, "template.IssuerIds");

            //发件人验证
            var senders = await _senderRepository.GetListAsync(new MatchIssuerByIdBatchSpecification<Issuer>(template.IssuerIds));

            IsNotNullOrEmpty(senders, "Issuers:" + template.IssuerIds);

            //系统发送者
            var sender = senders.FirstOrDefault(item => item.Name == request.Sender && item.Token == request.Token);

            if (sender == null)
            {
                throw new MessageCoreInternalException("000000", "Issuer or Token in Validate!" + request.Token);
            }

            //唯一性约束
            var isExist = await _orderRepository.ExistsAsync(new MatchOrderByReferenceNumberSpecification<Order>(request.ReferenceNumber));
            if (isExist)
            {
                throw new MessageCoreInternalException("000000", "referenceNumber is repeat!" + request.ReferenceNumber);
            }

            //保存附件
            var attachmentIds = new List<string>();
            if (request.Attachments != null && request.Attachments.Count > 0)
            {
                foreach (var item in request.Attachments)
                {
                    attachmentIds.Add(await _attachmentService.SaveAsync(item));
                }
            }

            //添加订单
            var order = new Order(request.ReferenceNumber, template.Id, JsonConvert.SerializeObject(request.Content), request.Sender, request.Token, attachmentIds, request.Receivers, request.Group);
            await _orderRepository.AddAsync(order);

            //这边可能有性能问题，后续考虑先返回给前端，再进行操作
            await SetSendingOrder(order.Id);
        }

        /// <summary>
        /// 添加待发送订单
        /// </summary>
        /// <param name="referenceNumber"></param>
        private async Task SetSendingOrder(string orderId)
        {
            //订单数据
            var order = await _orderRepository.GetByKeyAsync(orderId);

            IsNotNull(order, "order:" + orderId);

            try
            {
                //不存在的数据保存在订单下面
                var template = await _templateRepository.GetByKeyAsync(order.TemplateId);

                IsNotNull(template, "template:" + order.TemplateId);

                //模板没有开启
                if (!template.Enabled)
                {
                    throw new MessageCoreInternalException("000000", "templateName is not enabled!" + order.TemplateId);
                }

                //发送服务
                var sendingService = await _sendingServiceRepository.GetByKeyAsync(template.SendingServiceId);

                IsNotNull(sendingService, "sendingService:" + template.SendingServiceId);

                //主题，这边考虑，如果主题中没有绑定模板的话，就不需要使用这部分的功能。
                var themes = await _themeRepository.GetListAsync(new MatchThemeByTemplateIdSpecification<Theme>(order.TemplateId));
                
                //主题使用逻辑
                if (themes != null && themes.Count > 0)
                {
                    //开启主题
                    var enabledThemes = themes.Where(theme => theme.Enabled).ToList();
                    if (enabledThemes.Count < 1)
                    {
                        throw new MessageCoreInternalException("000000", "can not find Enabled theme!" + order.TemplateId);
                    }

                    //拦截
                    var blocking = await _blockingRepository.GetAsync(new MatchBlockingByTemplateNameSpecification<Blocking>(BlockingTemplateName));

                    //发送类型
                    var sendingServiceType = sendingService.SendingServiceType;

                    //过滤订单
                    var orderFilters = new List<OrderFilter>();

                    //待发送订单
                    var sendingOrders = new List<SendingOrder>();

                    foreach (var theme in themes)
                    {
                        //主题接收类型

                        IsNotNullOrEmpty(theme.SubscriberIds, "theme.SubscriberIds:theme name" + theme.Name);

                        var subscribers = await _subscriberRepository.GetListAsync(new MatchSubscriberByIdBatchSpecification<Subscriber>(theme.SubscriberIds));

                        IsNotNullOrEmpty(subscribers, "subscribers:theme name" + theme.Name);

                        foreach (var subscriber in subscribers)
                        {
                            //这边还要考虑黑名单的情况，由于现在的设计有点问题，等确认好再添加。
                            var receiveWay = GetReceiveWay(subscriber, sendingServiceType);

                            if (!subscriber.Enabled)
                            {
                                orderFilters.Add(new OrderFilter(order.Id, template.Id, theme.Id, sendingService.Id, receiveWay, FilterReasonType.SubscriberUnable));
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(receiveWay))
                                {
                                    orderFilters.Add(new OrderFilter(order.Id, template.Id, theme.Id, sendingService.Id, receiveWay, FilterReasonType.SubscriberReceiveWayNull));
                                }
                                else
                                {
                                    if (theme.SubscriberType != subscriber.SubscriberType)
                                    {
                                        orderFilters.Add(new OrderFilter(order.Id, template.Id, theme.Id, sendingService.Id, receiveWay, FilterReasonType.SubscriberTypeError));
                                    }
                                    else
                                    {
                                        if (blocking != null && blocking.Blacklists != null && blocking.Blacklists.Count > 0 && blocking.Blacklists.Any(item => item.Equals(receiveWay)))
                                        {
                                            orderFilters.Add(new OrderFilter(order.Id, template.Id, theme.Id, sendingService.Id, receiveWay, FilterReasonType.Blacklist));
                                        }
                                        else
                                        {

                                            var templateResult = await _templateAnalysisService.TemplateAnalysisAsync(template.TemplateInfo, JObject.Parse(order.ContentJson));
                                            sendingOrders.Add(new SendingOrder(order.Id, order.ReferenceNumber, sendingService.Id, sendingService.ServiceName, template.Id, template.TemplateName, order.SenderName, receiveWay, templateResult.Subject, templateResult.Header, templateResult.Content, templateResult.Footer, order.AttachmentIds, template.TimerType, SendingOrderStatus.Ready));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    await GenerateOrderReceivers(order, template, sendingService, sendingServiceType, orderFilters, sendingOrders);
                    await SaveSendingOrder(order, template, sendingService, sendingServiceType, orderFilters, sendingOrders);
                }
                else 
                {
                    //如果没有使用主题，则使用订单下面的数据进行发送。

                    //待发送订单
                    var sendingOrders = new List<SendingOrder>();

                    //过滤订单
                    var orderFilters = new List<OrderFilter>();

                    await GenerateOrderReceivers(order, template, sendingService, sendingService.SendingServiceType, orderFilters, sendingOrders);
                    await SaveSendingOrder(order, template, sendingService, sendingService.SendingServiceType, orderFilters, sendingOrders);

                }
            }
            catch (MessageCoreInternalException ex)
            {
                order.SetError(ex.Message, ex.Type);
            }
            catch (Exception ex)
            {
                _logger.LogError("SetSendingOrder Error:" + ex.ToString());
                order.SetError(ex.Message, ExceptionType.MessageCoreUnhandledException);
            }
        }

        private async Task SaveSendingOrder(Order? order, Template? template, SendingService? sendingService, SendingServiceType sendingServiceType, List<OrderFilter> orderFilters, List<SendingOrder> sendingOrders)
        {
            //保存数据库
            _unitOfWork.Begin();
            try
            {
                foreach (var filter in orderFilters)
                {
                    await _orderFilterRepository.AddAsync(filter);
                }

                foreach (var sendingOrder in sendingOrders)
                {
                    await _sendingOrderRepository.AddAsync(sendingOrder);
                }
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("SetSendingOrder unitofwork Error:" + ex.ToString());
                throw;
            }
        }

        private async Task GenerateOrderReceivers(Order? order, Template? template, SendingService? sendingService, SendingServiceType sendingServiceType, List<OrderFilter> orderFilters, List<SendingOrder> sendingOrders)
        {
            var templateResultForReceivers = await _templateAnalysisService.TemplateAnalysisAsync(template.TemplateInfo, JObject.Parse(order.ContentJson));

            //额外收件人信息
            foreach (var item in order.Receivers)
            {
                var receiveWay = GetReceiveWay(item, sendingServiceType);
                if (!string.IsNullOrWhiteSpace(receiveWay) && !sendingOrders.Where(item=>item.ReceiveWay.Equals(receiveWay)).Any())
                {
                    sendingOrders.Add(new SendingOrder(order.Id, order.ReferenceNumber, sendingService.Id, sendingService.ServiceName, template.Id, template.TemplateName, order.SenderName, receiveWay, templateResultForReceivers.Subject, templateResultForReceivers.Header, templateResultForReceivers.Content, templateResultForReceivers.Footer, order.AttachmentIds, template.TimerType, SendingOrderStatus.Ready));
                }
                else
                {
                    orderFilters.Add(new OrderFilter(order.Id, template.Id, "0", sendingService.Id, receiveWay, FilterReasonType.SubscriberReceiveWayNull));
                }
            }
        }

        private string GetReceiveWay(Domain.Common.ValueObjects.Receiver receiver, SendingServiceType sendingServiceType)
        {
            if (receiver == null) 
            {
                return "";
            }

            if (sendingServiceType == SendingServiceType.Email) 
            {
                return receiver.Email;
            }
            if (sendingServiceType == SendingServiceType.Phone)
            {
                return receiver.Phone;
            }
            if (sendingServiceType == SendingServiceType.EnterpriseWeChat)
            {
                return receiver.EnterpriseWeChat;
            }

            throw new MessageCoreInternalException("000000", "can not find "+ sendingServiceType.Name+ "in receiver.");
        }

        private string GetReceiveWay(Subscriber subscriber, SendingServiceType sendingServiceType)
        {
            var fieldName = sendingServiceType.Name;
            var propertyInfo = subscriber.GetType().GetProperty(fieldName);
            if (propertyInfo == null)
            {
                throw new MessageCoreInternalException("000000", "fieldName:" + fieldName + "Can not find in SendingServiceType");
            }
            return propertyInfo.GetValue(subscriber).ToString();
        }

        private void IsNotNullOrEmpty<T>(IList<T> argument, string argumentName)
        {
            if (argument == null || argument.Count <= 0)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.ValidateError, argumentName + "Collection Cannot Be Null Or Empty.");
            }
        }

        private void IsNotNull(object? argument, string argumentName)
        {
            if (argument == null)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.ValidateError, argumentName + " Cannot Be Null.");
            }
        }
    }
}
