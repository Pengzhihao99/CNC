using System;
using System.Collections.Generic;

namespace MessageCore.OpenApi.SDK.Definitions.Common
{
    public class ErrorData
    {
        /// <summary>
        /// 错误表示
        /// </summary>
        public string TicketId { get; set; }

        /// <summary>
        /// 错误发生时间
        /// </summary>
        public DateTime UtcDateTime { get; set; }

        /// <summary>
        /// 请求的URI
        /// </summary>
        public string RequestUri { get; set; }

        /// <summary>
        /// 错误详情列表
        /// </summary>
        public List<ErrorItem> Errors { get; set; }
    }
}
