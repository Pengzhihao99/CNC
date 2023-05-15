using MessageCore.Domain.Common.ValueObjects;
using MessageCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.OrderAggregate
{
    /// <summary>
    /// 采集订单表
    /// </summary>
    public class Order : AggregateRoot
    {
        public string ReferenceNumber { get; private set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public string TemplateId { get; private set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public string ContentJson { get; private set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string SenderName { get; private set; }

        /// <summary>
        /// 发送者token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public IList<string> AttachmentIds { get; private set; }

        /// <summary>
        /// 附件收件人
        /// </summary>
        public IList<Receiver> Receivers { get; private set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorType { get; private set; }

        /// <summary>
        /// 组，从属于，客户代码等
        /// </summary>
        public string Group { get; private set; }

        public void SetSuccess()
        {
            Success = true;
            ErrorMessage = "";
            SetUpdateOn();
        }

        public void SetError(string errorMessage, string errorType)
        {
            Success = false;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
            SetUpdateOn();
        }

        public Order(string referenceNumber, string templateId, string content, string senderName, string token,
            IList<string> attachmentIds, IList<Receiver> receivers, string group)
        {
            ReferenceNumber = referenceNumber;
            TemplateId = templateId;
            ContentJson = content;
            SenderName = senderName;
            Token = token;
            AttachmentIds = attachmentIds;
            Receivers = receivers;
            Group = group;

            SetSuccess();
        }
    }
}
