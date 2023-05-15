using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications
{
    public class MatchSendingOrderBySenderNameSpecification<T> : Specification<T>
 where T : SendingOrder
    {
        public string SenderName { get; private set; }
        public MatchSendingOrderBySenderNameSpecification(string senderName)
        {
            SenderName = senderName;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.SenderName.Contains(SenderName);
        }
    }
}
