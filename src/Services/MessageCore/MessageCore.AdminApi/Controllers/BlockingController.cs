using Framework.Application.Core.Queries;
using MediatR;
using MessageCore.Application.Admin.Commands.BlockingCommand;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.Queries;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MessageCore.AdminApi.Controllers
{
    [Route("api/v1/core/blockings")]
    public class BlockingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBlockingQueries _messageBlockingQueries;

        public BlockingController(IMediator mediator, IBlockingQueries messageBlockingQueries)
        {
            _mediator = mediator;
            _messageBlockingQueries = messageBlockingQueries;
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
        public async Task<IActionResult> AddBlockingAsync([FromBody] CreateBlockingCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateBlockingAsync([FromRoute] string id, [FromBody] UpdateBlockingCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BlockingVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBlockingByIdAsync([FromRoute] string id)
        {
            return Ok(await _messageBlockingQueries.GetBlockingByIdAsync(id));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(PagingFindResultWrapper<BlockingVM>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBlockingByPageAsync([FromQuery] BlockingQuery messageBlockingQuery)
        {
            return Ok(await _messageBlockingQueries.GetBlockingByPageAsync(messageBlockingQuery));
        }
    }
}
