using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Repositories
{
    public class CountResult
    {
        /// <summary>
        /// </summary>
        public CountResult(long count, long maxLimit, bool isGreaterThanMaxLimit)
        {
            Count = count;
            MaxLimit = maxLimit;
            IsGreaterThanMaxLimit = isGreaterThanMaxLimit;
        }

        /// <summary>
        /// 当前计数的数量 （若存在MaxLimit，则该值小于等于MaxLimit）
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        /// 最大值限制数量
        /// </summary>
        public long MaxLimit { get; private set; }

        /// <summary>
        /// 原始数量是否大于最大值限制数量 （假设数量10001，MaxLimit为10000，该值为True）
        /// </summary>
        public bool IsGreaterThanMaxLimit { get; private set; }
    }
}
