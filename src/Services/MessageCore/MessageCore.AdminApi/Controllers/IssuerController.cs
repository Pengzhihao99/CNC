using Framework.Application.Core.Queries;
using MediatR;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.Queries;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MessageCore.AdminApi.Controllers
{
    [ApiController]
    [Route("api/v1/core/issuers")]
    public class IssuerController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IIssuerQueries _senderQueries;

        public IssuerController(IMediator mediator, IIssuerQueries senderQueries)
        {
            _mediator = mediator;
            _senderQueries = senderQueries;
        }

       

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagingFindResultWrapper<IssuerVM>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetIssuersAsync([FromQuery] IssuerQuery query)
        {
            return Ok(await _senderQueries.GetIssuerByPageAsync(query));
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
        public async Task<IActionResult> AddIssuerAsync([FromBody] CreateIssuerCommand command)
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
        public async Task<IActionResult> UpdateIssuerAsync([FromBody] UpdateIssuerCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

    }
}
