using Framework.Domain.Core.Specification;
using MessageCore.Application.Services;
using MessageCore.Application.Services.Base;
using MessageCore.Domain.AggregatesModels.BlockingAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using MessageCore.Domain.Repositories;
using MessageCore.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.Common.Enum;

namespace MessageCore.BackgroundTask.Jobs.Recurring
{
    public class SendMessagesJob
    {
        private readonly ISendMessagesService _sendMessagesService;
        private readonly ISendingOrderRepository _sendingOrderRepository;

        private readonly ILogger<SendMessagesJob> _logger;
        

        public SendMessagesJob(ISendMessagesService sendMessagesService, ISendingOrderRepository sendingOrderRepository, ILogger<SendMessagesJob> logger)
        {
            _sendMessagesService = sendMessagesService;
            _sendingOrderRepository = sendingOrderRepository;
            _logger = logger;
        }


        public async Task ExecuteAsync()
        {
            //先查出立即待发送的
            var specification = new MatchSendingOrderBySendingOrderStatusSpecification<SendingOrder>(SendingOrderStatus.Ready);
            specification.And(new MatchSendingOrderByTimerTypeSpecification<SendingOrder>(TimerType.Immediately));
            var sendingOrders = await _sendingOrderRepository.GetListAsync(specification);

            //先全部改成发送中
            if (sendingOrders != null && sendingOrders.Count > 0)
            {
                foreach (var item in sendingOrders)
                {
                    item.SendingOrderStatus = SendingOrderStatus.Sending;
                    item.SetUpdateOn();
                }
                //更新为发送中
                await _sendingOrderRepository.UpdateAsync(sendingOrders);
            }
            
            var sendingOrdersNew = new List<SendingOrder>();
            foreach (var item in sendingOrders)
            {
                try
                {
                    //发送
                    await _sendMessagesService.SendAsync(item);
                    item.SetSuccess();
                    item.SetUpdateOn();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"邮件发送失败, OrderId:{item.OrderId}, 原因：{ex.Message}");
                    item.SetFail(ex.Message, "WebException");
                    item.SetUpdateOn();
                }
                sendingOrdersNew.Add(item);
            }

            //更新表为发送之后的数据
            if (sendingOrdersNew != null && sendingOrdersNew.Count > 0)
            {
                await _sendingOrderRepository.UpdateAsync(sendingOrdersNew);
            }
        }
    }
}
