using Framework.Application.Core.Queries;
using MediatR;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using MessageCore.Application.Admin.Commands.SendingServiceCommand;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.Queries;
using MessageCore.Application.Admin.Queries.Impl;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MessageCore.AdminApi.Controllers
{
    [ApiController]
    [Route("api/v1/core/sendingServices")]
    public class SendingServiceController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ISendingServiceQueries _sendingServiceQueries;

        public SendingServiceController(IMediator mediator, ISendingServiceQueries sendingServiceQueries)
        {
            _mediator = mediator;
            _sendingServiceQueries = sendingServiceQueries;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagingFindResultWrapper<SendingServiceVM>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetSendingServiceAsync([FromQuery] SendingServiceQuery query)
        {
            return Ok(await _sendingServiceQueries.GetSendingServiceByPageAsync(query));
        }

        /// <summary>
        /// 查询serviceNames
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("serviceNames")]
        [ProducesResponseType(typeof(List<SendingServiceNamesVM>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetSendingServiceNameAllAsync()
        {
            return Ok(await _sendingServiceQueries.GetSendingServiceNameAllAsync());
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddSendingServiceAsync([FromBody] CreateSendingServiceCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateSenderAsync([FromBody] UpdateSendingServiceCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
