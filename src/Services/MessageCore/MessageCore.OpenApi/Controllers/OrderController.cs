using MediatR;
using MessageCore.Application.OpenApi.Commands.OrderCommand;
using MessageCore.Application.OpenApi.DataTransferModels;
using MessageCore.Domain.Common.ValueObjects;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Scriban.Parsing;
using System.Net;
using System.Net.Mail;

namespace MessageCore.OpenApi.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 添加通知消息订单
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddOrderAsync([FromBody] CreateOrderCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 添加简易邮件消息订单
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SimpleEmail")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddSimpleEmailOrderAsync([FromBody] CreateSimpleEmailCommand command)
        {
            var simpleCommand = new CreateOrderCommand
            {
                ReferenceNumber = command.ReferenceNumber,
                TemplateName = "GenericEmailTemplate",
                Content = new { command.Subject, command.Content },
                Sender = command.Sender,
                Token = command.Token,
                Receivers = command.Receivers.Select(item => new Receiver(item, item, "", "")).ToList(),
                Attachments = command.Attachments?.Select(x => new AttachmentDto
                {
                    AttachmentType = x.AttachmentType,
                    Data = x.Data,
                    Name = x.Name,
                }).ToList() ?? new List<AttachmentDto>()

            };
            await _mediator.Send(simpleCommand);
            return Ok();
        }

        /// <summary>
        ///添加简易企业微信消息订单
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SimpleEnterpriseWeChat")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddSimpleEnterpriseWeChatOrderAsync([FromBody] CreateSimpleEnterpriseWeChatCommand command)
        {
            var simpleCommand = new CreateOrderCommand
            {
                ReferenceNumber = command.ReferenceNumber,
                TemplateName = "GenericWeChatTemplate",
                Content = new { command.Content },
                Sender = command.Sender,
                Token = command.Token,
                Receivers = command.Receivers.Select(item => new Receiver(item, "", "", item)).ToList()
            };
            await _mediator.Send(simpleCommand);
            return Ok();
        }
    };
}