using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Entities
{
    /// <summary>
    /// 表示存在 并发戳
    /// </summary>
    public interface IHasConcurrencyStamp
    {
        /// <summary>
        /// 并发戳
        /// 避免并发问题所必需的，类似于RowVersion
        /// </summary>
        string ConcurrencyStamp { get; set; }
    }
}
