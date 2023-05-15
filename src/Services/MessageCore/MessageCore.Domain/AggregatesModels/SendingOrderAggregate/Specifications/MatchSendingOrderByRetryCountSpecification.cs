using Framework.Domain.Core.Specification;
using MessageCore.Domain.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications
{
    public class MatchSendingOrderByRetryCountSpecification<T> : Specification<T>
   where T : SendingOrder
    {
        public int RetryCount { get; private set; }
        public MatchSendingOrderByRetryCountSpecification(int retryCount)
        {
            RetryCount = retryCount;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.RetryCount < RetryCount;
        }
    }
}
