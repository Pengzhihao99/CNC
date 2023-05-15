using Framework.Application.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.DataTransferModels
{
    public class ThemeQuery : AbstractPagingFindQuery
    {
        /// <summary>
        /// 主题Id
        /// </summary>
        public string? Id { get; set; }
    }
}
