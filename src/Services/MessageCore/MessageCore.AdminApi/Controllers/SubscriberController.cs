using Framework.Application.Core.Queries;
using MediatR;
using MessageCore.Application.Admin.Commands.SubscriberCommand;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.Queries;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MessageCore.AdminApi.Controllers;

[ApiController]
[Route("api/v1/core/subscribers")]
public class SubscriberController : Controller
{
    private readonly IMediator _mediator;
    private readonly ISubscriberQueries _subscriberQueries;

    public SubscriberController(ISubscriberQueries subscriberQueries, IMediator mediator)
    {
        _subscriberQueries = subscriberQueries;
        _mediator = mediator;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(PagingFindResultWrapper<SubscriberVM>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetSubscribersAsync([FromQuery] SubscriberQuery query)
    {
        return Ok(await _subscriberQueries.GetSubscribersByPageAsync(query));
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
    public async Task<IActionResult> AddSubscriberAsync(CreateSubscriberCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut("")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateSubscriberAsync(UpdateSubscriberCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}