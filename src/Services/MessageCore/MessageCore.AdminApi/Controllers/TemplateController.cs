using Framework.Application.Core.Queries;
using MediatR;
using MessageCore.Application.Admin.Commands.TemplateCommand;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.Queries;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MessageCore.AdminApi.Controllers
{
    [ApiController]
    [Route("api/v1/core/templates")]
    public class TemplateController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITemplateQueries _templateQueries;

        public TemplateController(IMediator mediator, ITemplateQueries templateQueries)
        {
            _mediator = mediator;
            _templateQueries = templateQueries;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagingFindResultWrapper<TemplateVM>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTemplatesAsync([FromQuery] TemplateQuery query)
        {
            return Ok(await _templateQueries.GetTemplateByPageAsync(query));
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
        public async Task<IActionResult> AddTemplateAsync([FromBody] CreateTemplateCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateTemplateAsync([FromBody] UpdateTemplateCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 模板测试
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("test")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> TemplateForTestAsync([FromBody] TemplateForTestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
