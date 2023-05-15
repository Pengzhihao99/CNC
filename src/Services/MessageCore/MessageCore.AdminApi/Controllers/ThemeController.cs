using MediatR;
using MessageCore.Application.Admin.Commands.MessageSenderCommand;
using MessageCore.Application.Admin.Commands.ThemeCommand;
using MessageCore.Application.Admin.Queries;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MessageCore.AdminApi.Controllers
{
    [ApiController]
    [Route("api/v1/core/themes")]
    public class ThemeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IThemeQueries _themeQueries;

        public ThemeController(IMediator mediator, IThemeQueries themeQueries)
        {
            _mediator = mediator;
            _themeQueries = themeQueries;
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
        public async Task<IActionResult> AddThemeAsync([FromBody] CreateThemeCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
