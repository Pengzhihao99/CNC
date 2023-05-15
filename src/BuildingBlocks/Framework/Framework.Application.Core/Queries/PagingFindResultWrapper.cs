using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Core.Queries
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingFindResultWrapper<T>
    {
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long TotalRecords { get; set; }

        /// <summary>
        /// 总的页面数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 获取或设置页码
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 获取或设置页面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}
