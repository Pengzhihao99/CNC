using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.OrderFilterAggregate
{
    public class OrderFilter : AggregateRoot
    {
        /// <summary>
        /// 关联主订单
        /// </summary>
        public string OrderId { get; private set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public string TemplateId { get; private set; }

        /// <summary>
        /// 主题Id
        /// </summary>
        public string ThemeId { get; private set; }

        /// <summary>
        /// 发送服务Id
        /// </summary>
        public string SendingServiceId { get; set; }

        /// <summary>
        /// 收信方式
        /// </summary>
        public string ReceiveWay { get; private set; }

        /// <summary>
        /// 过滤原因
        /// </summary>
        public FilterReasonType FilterReasonType { get; private set; }

        public string Message { get; set; }

        public OrderFilter(string orderId, string templateId, string themeId, string sendingServiceId, string receiveWay, FilterReasonType filterReasonType)
        {
            OrderId = orderId;
            TemplateId = templateId;
            ThemeId = themeId;
            SendingServiceId = sendingServiceId;
            ReceiveWay = receiveWay;
            FilterReasonType = filterReasonType;
        }
    }
}
