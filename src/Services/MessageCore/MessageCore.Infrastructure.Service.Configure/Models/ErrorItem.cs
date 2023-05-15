using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Service.Configure.Models
{
    /// <summary>
    /// 错误项
    /// </summary>
    public class ErrorItem
    {
        public ErrorItem(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public ErrorItem(string sn, string code, string message)
        {
            this.Sn = sn;
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        public string Sn { get; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; }
    }
}
