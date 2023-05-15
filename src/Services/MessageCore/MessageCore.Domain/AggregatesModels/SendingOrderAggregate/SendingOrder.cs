using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingOrderAggregate
{
    /// <summary>
    /// 待发送订单表
    /// </summary>
    public class SendingOrder : AggregateRoot
    {
        /// <summary>
        /// 关联主订单
        /// </summary>
        public string OrderId { get; private set; }

        /// <summary>
        /// 主订单参考号
        /// </summary>
        public string ReferenceNumber { get; private set; }

        /// <summary>
        /// 服务Id，拆分队列
        /// </summary>
        public string ServiceId { get; private set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; private set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string SenderName { get; private set; }

        /// <summary>
        /// 模板Id（用于对标题分组，还要考虑有可能中途修改的情况），所以要做两层过滤
        /// </summary>
        public string TemplateId { get; private set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; private set; }

        /// <summary>
        /// 收信方式
        /// </summary>
        public string ReceiveWay { get; private set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; private set; }

        /// <summary>
        /// 内容头
        /// </summary>
        public string ContentHeader { get; private set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 内容尾
        /// </summary>
        public string ContentFooter { get; private set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public IList<string> AttachmentIds { get; private set; }

        /// <summary>
        /// 定时器类型
        /// </summary>
        public TimerType TimerType { get; private set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public SendingOrderStatus SendingOrderStatus { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; private set; }

        /// <summary>
        /// 错误类型
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        public SendingOrder(string orderId,string referenceNumber, string serviceId, string serviceName, string templateId, string templateName, string senderName, string receiveWay, string subject, 
            string contentHeader, string content, string contentFooter, IList<string> attachmentIds, 
            TimerType timerType, SendingOrderStatus sendingOrderStatus)
        {
            OrderId = orderId;
            ReferenceNumber = referenceNumber;
            ServiceId = serviceId;
            ServiceName = serviceName;
            TemplateId = templateId;
            TemplateName = templateName;

            SenderName = senderName;
            ReceiveWay = receiveWay;

            Subject = subject;
            ContentHeader = contentHeader;
            Content = content;
            ContentFooter = contentFooter;

            AttachmentIds = attachmentIds;

            TimerType = timerType;

            SendingOrderStatus = sendingOrderStatus;
            RetryCount = 0;
            ErrorType = "";
            ErrorMessage = "";
        }

        public void SetSuccess()
        {
            SendingOrderStatus = SendingOrderStatus.Success;
        }

        public void SetRetrySuccess()
        {
            RetryCount++;
            SendingOrderStatus = SendingOrderStatus.Success;
        }

        public void SetSending()
        {
            SendingOrderStatus = SendingOrderStatus.Sending;
        }

        public void SetFail(string errorMessage, string errorType)
        {
            SendingOrderStatus = SendingOrderStatus.Fail;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
        }

        public void SetRetryFail(string errorMessage, string errorType)
        {
            RetryCount++;
            SendingOrderStatus = SendingOrderStatus.Fail;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
        }
    }
}
