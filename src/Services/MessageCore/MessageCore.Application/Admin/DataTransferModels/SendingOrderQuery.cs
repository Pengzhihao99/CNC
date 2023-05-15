using Framework.Application.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.DataTransferModels
{
    public class SendingOrderQuery : AbstractPagingFindQuery
    {
        /// <summary>
        /// 参考号
        /// </summary>
        public string ReferenceNumbers { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string? SenderName { get; set; }
    }
}
