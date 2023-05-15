using Framework.Domain.Core.Specification;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications
{
    public class MatchSendingOrderBySendingOrderStatusSpecification<T> : Specification<T>
    where T : SendingOrder
    {
        public SendingOrderStatus SendingOrderStatus { get; private set; }
        public MatchSendingOrderBySendingOrderStatusSpecification(SendingOrderStatus sendingOrderStatus)
        {
            SendingOrderStatus = sendingOrderStatus;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.SendingOrderStatus == SendingOrderStatus;
        }
    }
}
