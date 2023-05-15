using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingServiceAggregate.Specifications
{
    public class MatchSendingServiceByServiceNameSpecification<T> : Specification<T>
    where T : SendingService
    {
        public string ServiceName { get; private set; }
        public MatchSendingServiceByServiceNameSpecification(string serviceName)
        {
            ServiceName = serviceName;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.ServiceName == ServiceName;
        }
    }
}
