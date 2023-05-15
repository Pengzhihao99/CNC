using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Repositories
{
    public class SortCriteria<TAggregateRoot>
       where TAggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// </summary>
        public SortCriteria(Expression<Func<TAggregateRoot, dynamic>> sortKeySelector, SortOrder sortOrder)
        {
            SortKeySelector = sortKeySelector;
            SortOrder = sortOrder;
        }

        /// <summary>
        /// 提取排序键的委托函数
        /// </summary>
        public Expression<Func<TAggregateRoot, dynamic>> SortKeySelector { get; private set; }

        /// <summary>
        /// 排序顺序(升序/降序)
        /// </summary>
        public SortOrder SortOrder { get; private set; }

    }
}
