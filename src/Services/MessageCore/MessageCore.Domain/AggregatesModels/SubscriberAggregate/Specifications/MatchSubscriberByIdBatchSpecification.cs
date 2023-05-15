using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications
{
    public class MatchSubscriberByIdBatchSpecification<T> : Specification<T>
    where T : Subscriber
    {
        public IEnumerable<string> Ids { get; private set; }
        public MatchSubscriberByIdBatchSpecification(IEnumerable<string> ids)
        {
            Ids = ids;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            if (Ids == null || Ids.Count() < 1)
            {
                return account => false;
            }
            else
            {
                return account => Ids.Contains(account.Id);
            }
        }
    }
}