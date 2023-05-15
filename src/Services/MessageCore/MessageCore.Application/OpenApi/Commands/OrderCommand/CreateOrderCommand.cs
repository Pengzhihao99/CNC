
using System.Collections.Generic;
#if SDK
namespace MessageCore.OpenApi.SDK.Definitions
{
public class CreateOrderCommand
#else
using MessageCore.Application.OpenApi.DataTransferModels;
using Framework.Application.Core.Commands;
using MediatR;
using MessageCore.Domain.Common.ValueObjects;
using MessageCore.Application.OpenApi.DataTransferModels;
namespace MessageCore.Application.OpenApi.Commands.OrderCommand
{
public class CreateOrderCommand : IRequest, ICommand
#endif
{
        /// <summary>
        /// 参考号
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 模板内容数据
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 发送者token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 追加收件人
        /// </summary>
        public List<Receiver> Receivers { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<AttachmentDto>? Attachments { get; set; }

        /// <summary>
        /// 组，从属于，客户代码，暂时不考虑
        /// </summary>
        public string? Group { get; set; }
    }
}
