using Framework.Infrastructure.Crosscutting.Exceptions;
using Framework.Infrastructure.Crosscutting.Extensions;
using MessageCore.Infrastructure.Exceptions;
using MessageCore.Infrastructure.Service.Configure.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MessageCore.AdminApi.Extensions
{
    /// <summary>
    /// 全局异常处理器
    /// </summary>
    public class ExceptionHttpHandlerMiddleware
    {
        /// <summary>
        /// 下一个中间件
        /// </summary>
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHttpHandlerMiddleware> _logger;

        /// <summary>
        /// 在每个应用程序生存期构造一次中间件
        /// </summary>
        /// <param name="next"></param>
        /// <param name="jsonConverter"></param>
        /// <param name="logger"></param>
        public ExceptionHttpHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHttpHandlerMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        /// <summary>
        /// 按请求中的中间件共享服务，需要在这边注入
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            try
            {
                await _next(context); //执行下一个中间件
            }
            catch (System.Exception ex)
            {
                var ticketId = Guid.NewGuid().ToString();
                var requestToString = await RequestToStringAsync(context.Request, ticketId);
                _logger.LogError(ex, requestToString);
                await HandleExceptionAsync(context, ex, ticketId);
            }
        }

        /// <summary>
        /// 处理异常信息
        /// </summary>
        private Task HandleExceptionAsync(HttpContext context, System.Exception ex, string ticketId)
        {
            // 自定义异常
            if (ex is MessageCoreInternalException internalException)
            {
                return GenerarteCustomerException(context, ticketId, internalException);
            }
            else if (ex is MessageCoreExternalException externalException)
            {
                return GenerarteCustomerException(context, ticketId, externalException);
            }
            else if (ex is ArgumentException)
            {
                var metadata = new ErrorData(string.Empty, string.Format("输入参数异常：{0}", ex.Message), context.Request.GetAbsoluteUri())
                {
                    TicketId = ticketId,
                };

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ContentType = "application/json;charset=utf-8";
                return context.Response.WriteAsync(JsonConvert.SerializeObject(metadata));
            }

            //序列化异常
            else if (ex is System.Runtime.Serialization.SerializationException)
            {
                var metadata = new ErrorData(string.Empty, string.Format("序列化发生异常，可能提交的数据格式有误：{0}", ex.Message), context.Request.GetAbsoluteUri())
                {
                    TicketId = ticketId,
                };

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ContentType = "application/json;charset=utf-8";
                return context.Response.WriteAsync(JsonConvert.SerializeObject(metadata));
            }
            //未处理异常
            else
            {
                var metadata = new ErrorData(string.Empty, string.Format("抱歉！发现系统异常，请您用此TicketId:{0}联系系统管理员，我们将以最快的速度为您解决此问题，谢谢！", ticketId), context.Request.GetAbsoluteUri())
                {
                    TicketId = ticketId,
                };

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.ContentType = "application/json;charset=utf-8";
                return context.Response.WriteAsync(JsonConvert.SerializeObject(metadata));
            }
        }

        private Task GenerarteCustomerException(HttpContext context, string ticketId, ExceptionBase ex)
        {
            var errorCode = ex.ErrorCode;
            var errorMsg = ex.CustomMessage;

            var metadata = new ErrorData(errorCode, errorMsg, context.Request.GetAbsoluteUri())
            {
                TicketId = ticketId,
            };

            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ContentType = "application/json;charset=utf-8";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(metadata));
        }

        private async Task<string> RequestToStringAsync(HttpRequest request, string ticketId = "")
        {
            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(ticketId))
            {
                message.AppendLine($"[TicketId]: {ticketId} ");
            }

            if (request.Method != null)
            {
                message.AppendLine($"[Method]: {request.Method} ");
            }

            message.AppendLine($"[RequestUri]: {request.GetAbsoluteUri()} ");

            //Header
            if (request.Headers != null)
            {
                message.AppendLine("[Header] Values: ");
                foreach (var headerItem in request.Headers)
                {
                    message.AppendLine($"--> [{headerItem.Key}]: {headerItem.Value} ");
                }
            }

            //Body
            if (request.Method.NotNullOrBlank() && !request.Method.EqualsIgnoreCase("GET") &&
                 request.Headers != null &&
                !request.Headers["Content-Type"].ToString().ContainsIgnoreCase("multipart/form-data"))
            {
                var bodyContent = string.Empty;

                try
                {
                    await request.Body.DrainAsync(CancellationToken.None);
                    request.Body.Seek(0L, SeekOrigin.Begin);
                    await using (var ms = new MemoryStream(2048))
                    {
                        request.Body.Position = 0;
                        request.Body.CopyTo(ms);
                        var content = ms.ToArray();
                        bodyContent = Encoding.UTF8.GetString(content);
                    }
                    request.Body.Seek(0L, SeekOrigin.Begin);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Get RequestBody Error.");
                }

                message.AppendLine("[Body]: ");
                message.AppendLine($"--> {bodyContent}");

            }
            return message.ToString();
        }
    }
}
