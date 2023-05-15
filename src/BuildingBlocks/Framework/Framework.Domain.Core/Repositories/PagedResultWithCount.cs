using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Repositories
{
    /// <summary>
    /// 表示一个单页数据分页的信息的集合类型，同时包含了数据的总计数信息。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResultWithCount<T> : PagedResult<T>
    {
        /// <summary>
        /// </summary> 
        public PagedResultWithCount(long totalRecords, int totalPages, int pageNumber, int pageSize, List<T> data) : base(pageNumber, pageSize, data)
        {
            this.TotalPages = totalPages;
            this.TotalRecords = totalRecords;
        }

        /// <summary>
        /// 获取或设置总记录数。
        /// </summary>
        public long TotalRecords { get; private set; }

        /// <summary>
        /// 获取或设置页数。
        /// </summary>
        public int TotalPages { get; private set; }
    }
}
