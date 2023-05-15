using MessageCore.Domain.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.ViewModels
{
    public class SendingServiceNamesVM
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 发送服务类型
        /// </summary>
        public SendingServiceType SendingServiceType { get; set; }
    }
}
