using Framework.Domain.Core.Specification;
using MessageCore.Domain.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications
{
    public class MatchSendingOrderByTimerTypeSpecification<T> : Specification<T>
    where T : SendingOrder
    {
        public TimerType TimerType { get; private set; }
        public MatchSendingOrderByTimerTypeSpecification(TimerType timerType)
        {
            TimerType = timerType;
        }
        public override Expression<Func<T, bool>> GetExpression()
        {
            return item => item.TimerType == TimerType;
        }
    }
}
