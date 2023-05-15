
using System.Collections.Generic;
#if SDK
namespace MessageCore.OpenApi.SDK.Definitions
#else
using MessageCore.Application.OpenApi.DataTransferModels;
namespace MessageCore.Application.OpenApi.Commands.OrderCommand
#endif
    {
    public class CreateSimpleEmailCommand
    {
        /// <summary>
        /// 参考号
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 发送者token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public List<string> Receivers { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<AttachmentDto>? Attachments { get; set; }
    }

    
}
