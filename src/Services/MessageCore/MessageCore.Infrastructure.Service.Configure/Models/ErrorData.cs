using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Service.Configure.Models
{
    /// <summary>
    /// 错误数据
    /// </summary>
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

        public ErrorData()
        {
            this.UtcDateTime = System.DateTime.UtcNow;
            this.Errors = new List<ErrorItem>();
        }

        public ErrorData(string errorCode, string errorMessage)
            : this(errorCode, errorMessage, String.Empty)
        {

        }

        public ErrorData(string errorCode, string errorMessage, string requestUri)
            : this()
        {
            this.UtcDateTime = System.DateTime.UtcNow;
            this.Errors = new List<ErrorItem> { new ErrorItem(errorCode, errorMessage) };
            this.RequestUri = requestUri;
        }
    }
}
