using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain.Core.Specification;

namespace MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications
{
    public class MatchSubscriberByGroupSpecification<T> : Specification<T>
        where T : Subscriber
    {
        /// <summary>
        /// 组，从属于，客户代码
        /// </summary>
        public string Group { get;private set; }

        public MatchSubscriberByGroupSpecification(string group)
        {
            Group = group;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.Group.Contains(Group);
        }
    }
}
