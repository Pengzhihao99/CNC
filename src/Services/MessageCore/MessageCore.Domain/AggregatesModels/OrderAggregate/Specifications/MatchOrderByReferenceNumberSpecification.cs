using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.OrderAggregate.Specifications
{
    public class MatchOrderByReferenceNumberSpecification<T> : Specification<T>
        where T : Order
    {
        public string ReferenceNumber { get; private set; }
        public MatchOrderByReferenceNumberSpecification(string referenceNumber)
        {
            ReferenceNumber = referenceNumber;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.ReferenceNumber.Contains(ReferenceNumber);
        }
    }
}
