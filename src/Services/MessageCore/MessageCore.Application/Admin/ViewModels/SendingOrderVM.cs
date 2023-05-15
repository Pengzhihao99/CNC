using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.ViewModels
{
    /// <summary>
    /// 消息订单视图
    /// </summary>
    public class SendingOrderVM
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 主订单参考号
        /// </summary>
        public string ReferenceNumber { get; private set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string ReceiveWay { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 头
        /// </summary>
        public string ContentHeader { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 尾
        /// </summary>
        public string ContentFooter { get; set; }

        /// <summary>
        /// 定时器类型
        /// </summary>
        public string TimerType { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string SendingOrderStatus { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 错误类型
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateOn { get; set; }
    }
}
