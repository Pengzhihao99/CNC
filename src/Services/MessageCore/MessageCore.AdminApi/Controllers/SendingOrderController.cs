using Framework.Application.Core.Queries;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.Queries;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MessageCore.AdminApi.Controllers
{
    [Route("api/v1/core/sendingOrders")]
    public class SendingOrderController : Controller
    {
        private readonly ISendingOrderQueries _sendingOrderQueries;

        public SendingOrderController(ISendingOrderQueries sendingOrderQueries)
        {
            _sendingOrderQueries = sendingOrderQueries;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(PagingFindResultWrapper<BlockingVM>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorData), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBlockingByPageAsync([FromQuery] SendingOrderQuery sendingOrderQuery)
        {
            return Ok(await _sendingOrderQueries.GetSendingOrderByIdOrSenderNameAsync(sendingOrderQuery));
        }
    }
}
