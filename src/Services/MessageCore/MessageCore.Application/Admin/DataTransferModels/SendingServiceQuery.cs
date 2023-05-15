using Framework.Application.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.DataTransferModels
{
    public class SendingServiceQuery : AbstractPagingFindQuery
    {
        /// <summary>
        /// 服务名
        /// </summary>
        public string? ServiceName { get; set; }
    }
}
