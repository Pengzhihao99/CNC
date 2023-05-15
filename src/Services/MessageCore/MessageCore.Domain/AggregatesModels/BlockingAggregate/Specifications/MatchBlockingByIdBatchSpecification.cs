using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.BlockingAggregate.Specifications
{

    public class MatchBlockingByIdBatchSpecification<T> : Specification<T>
      where T : Blocking
    {
        public IEnumerable<string> Ids { get; private set; }
        public MatchBlockingByIdBatchSpecification(IEnumerable<string> ids)
        {
            Ids = ids;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            if (Ids == null || Ids.Count() < 1)
            {
                return item => false;
            }
            else
            {
                return item => Ids.Contains(item.Id);
            }
        }
    }
}
