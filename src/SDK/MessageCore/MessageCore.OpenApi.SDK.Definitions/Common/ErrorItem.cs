using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.OpenApi.SDK.Definitions.Common
{
    /// <summary>
    /// 错误项
    /// </summary>
    public class ErrorItem
    {
        /// <summary>
        /// 单据编号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
    }
}
