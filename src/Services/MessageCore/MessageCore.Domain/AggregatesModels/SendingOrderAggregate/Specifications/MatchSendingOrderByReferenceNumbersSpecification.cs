using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications
{
    public class MatchSendingOrderByReferenceNumbersSpecification<T> : Specification<T>
 where T : SendingOrder
    {
        public List<string> ReferenceNumbers { get; private set; }
        public MatchSendingOrderByReferenceNumbersSpecification(List<string> referenceNumbers)
        {
            ReferenceNumbers = referenceNumbers;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            if (ReferenceNumbers == null || ReferenceNumbers.Count < 1)
            {
                return order => false;
            }
            else
            {
                return order => ReferenceNumbers.Contains(order.ReferenceNumber);
            }
        }
    }
}
