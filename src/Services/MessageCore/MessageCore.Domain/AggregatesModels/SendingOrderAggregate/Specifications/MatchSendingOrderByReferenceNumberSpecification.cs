using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications
{
    public class MatchSendingOrderByReferenceNumberSpecification<T> : Specification<T>
  where T : SendingOrder
    {
        public string ReferenceNumber { get; private set; }
        public MatchSendingOrderByReferenceNumberSpecification(string referenceNumber)
        {
            ReferenceNumber = referenceNumber;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.ReferenceNumber.Contains(ReferenceNumber);
        }
    }
}
