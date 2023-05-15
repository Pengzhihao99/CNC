using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingServiceAggregate.Specifications
{
    public class MatchSendingServiceByEnabledSpecification<T> : Specification<T>
   where T : SendingService
    {
        public bool Enabled { get; private set; }
        public MatchSendingServiceByEnabledSpecification(bool enabled)
        {
            Enabled = enabled;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.Enabled == Enabled;
        }
    }
}
