using System.Collections.Generic;
#if SDK
namespace MessageCore.OpenApi.SDK.Definitions
#else
namespace MessageCore.Application.OpenApi.Commands.OrderCommand
#endif
{
    public class CreateSimpleEnterpriseWeChatCommand
    {
        /// <summary>
        /// 参考号
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// 发信内容
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
        /// 收件人企业微信ID，通常是员工号
        /// </summary>
        public List<string> Receivers { get; set; }
    }

}
