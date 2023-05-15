using MessageCore.Application.Services.Base;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using MessageCore.Infrastructure.Exceptions;
using Newtonsoft.Json.Linq;
using MessageCore.Domain.AggregatesModels.SendingAttachmentAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.AttachmentAggregate;
using Org.BouncyCastle.Utilities;
using MessageCore.Application.OpenApi.DataTransferModels;
using MessageCore.Domain.Common.Enum;
using MessageCore.Application.SendingServices.Base;
using MessageCore.Application.DataTransferModels;

namespace MessageCore.Application.Services
{
    public class SendMessagesService : ISendMessagesService
    {
        private readonly ISendingServiceRepository _sendingServiceRepository;
        private readonly ISendingAttachmentRepository _sendingAttachmentRepository;

        //这边要考虑后续有多个实现时需要怎么处理
        private readonly Lazy<IEnumerable<IEmailService>> _emailServices;
        private readonly Lazy<IEnumerable<IEnterpriseWeChatService>> _enterpriseWeChatService;
        private readonly Lazy<IEnumerable<ISMSService>> _smsService;

        public SendMessagesService(ISendingServiceRepository sendingServiceRepository, ISendingAttachmentRepository sendingAttachmentRepository,
            Lazy<IEnumerable<IEmailService>> emailServices, Lazy<IEnumerable<IEnterpriseWeChatService>> enterpriseWeChatService, Lazy<IEnumerable<ISMSService>> smsService)
        {
            _sendingServiceRepository = sendingServiceRepository;
            _sendingAttachmentRepository = sendingAttachmentRepository;

            _emailServices = emailServices;
            _enterpriseWeChatService = enterpriseWeChatService;
            _smsService = smsService;
        }

        public async Task SendAsync(SendingOrder sendingOrder)
        {
            //服务配置
            var serviceInfo = await _sendingServiceRepository.GetByKeyAsync(sendingOrder.ServiceId);
            if (!serviceInfo.Enabled)
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.SendingServiceEnabledError, string.Format(ErrorMessage.SendingServiceEnabledError, serviceInfo.ServiceName));
            }

            if (serviceInfo.SendingServiceType == SendingServiceType.Email)
            {
                var emailServices = (_emailServices.Value?.FirstOrDefault()) ?? throw new MessageCoreInternalException(ErrorCode.StringCode.EmailServiceNotFound, ErrorMessage.EmailServiceNotFound);

                //附件
                var attachmentDatas = await _sendingAttachmentRepository.GetListAsync(new MatchSendingAttachmentByIdsSpecification<SendingAttachment>(sendingOrder.AttachmentIds));
                await emailServices.SendAsync(
                    new From() { Address = serviceInfo.Sender },
                    new To() { Address = sendingOrder.ReceiveWay },
                    sendingOrder.Subject,
                    (sendingOrder.ContentHeader + sendingOrder.Content + sendingOrder.ContentFooter),
                    attachmentDatas,
                    new Account()
                    {
                        Host = serviceInfo.Host,
                        UserName = serviceInfo.UserName,
                        Password = serviceInfo.PassWord
                    });
            }
            else if (serviceInfo.SendingServiceType == SendingServiceType.Phone)
            {
                var smsService = (_smsService.Value?.FirstOrDefault()) ?? throw new MessageCoreInternalException(ErrorCode.StringCode.SmsServiceNotFound, ErrorMessage.SmsServiceNotFound);
            }
            else if (serviceInfo.SendingServiceType == SendingServiceType.EnterpriseWeChat)
            {
                var enterpriseWeChatService = (_enterpriseWeChatService.Value?.FirstOrDefault()) ?? throw new MessageCoreInternalException(ErrorCode.StringCode.EnterpriseWeChatServiceNotFound, ErrorMessage.EnterpriseWeChatServiceNotFound);
                await enterpriseWeChatService.SendAsync(
                    new From() { Address = serviceInfo.Sender },
                    new To() { Address = sendingOrder.ReceiveWay },
                    (string.IsNullOrWhiteSpace(sendingOrder.ContentHeader) ? "" : (sendingOrder.ContentHeader + "\n"))
                    + (string.IsNullOrWhiteSpace(sendingOrder.Content) ? "" : (sendingOrder.Content + "\n"))
                    + sendingOrder.ContentFooter,
                    new Account()
                    {
                        Host = serviceInfo.Host,
                        UserName = serviceInfo.UserName,
                        Password = serviceInfo.PassWord
                    });
            }
            else
            {
                throw new MessageCoreInternalException(ErrorCode.StringCode.SendingServiceTypeError, ErrorMessage.SendingServiceTypeError);
            }
        }
    }
}
