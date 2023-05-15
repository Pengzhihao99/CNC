using MessageCore.Application.Services.Base;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Repositories;

namespace MessageCore.BackgroundTask.Jobs.Recurring
{
    public class SendMessagesRetryJob
    {
        private readonly ISendMessagesService _sendMessagesService;
        private readonly ISendingOrderRepository _sendingOrderRepository;

        private readonly ILogger<TestJob> _logger;


        public SendMessagesRetryJob(ISendMessagesService sendMessagesService, ISendingOrderRepository sendingOrderRepository, ILogger<TestJob> logger)
        {
            _sendMessagesService = sendMessagesService;
            _sendingOrderRepository = sendingOrderRepository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            //先查出立即发送失败的
            var specification = new MatchSendingOrderBySendingOrderStatusSpecification<SendingOrder>(SendingOrderStatus.Fail);
            specification.And(new MatchSendingOrderByTimerTypeSpecification<SendingOrder>(TimerType.Immediately));
            //最多重试五次
            specification.And(new MatchSendingOrderByRetryCountSpecification<SendingOrder>(5));

            var sendingOrders = await _sendingOrderRepository.GetListAsync(specification);

            //先全部改成发送中
            if (sendingOrders != null && sendingOrders.Count > 0)
            {
                foreach (var item in sendingOrders)
                {
                    item.SetSending();
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
                    //发送通知
                    await _sendMessagesService.SendAsync(item);
                    item.SetRetrySuccess();
                    item.SetUpdateOn();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"通知发送失败, OrderId:{item.OrderId}, 原因：{ex.Message}");
                    item.SetRetryFail(ex.Message, "WebException");
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
