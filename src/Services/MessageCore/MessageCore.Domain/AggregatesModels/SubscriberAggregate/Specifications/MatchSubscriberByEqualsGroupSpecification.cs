using System.Linq.Expressions;
using Framework.Domain.Core.Specification;

namespace MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications;

public class MatchSubscriberByEqualsGroupSpecification<T> : Specification<T>
    where T : Subscriber
{
    /// <summary>
    /// 组，从属于，客户代码
    /// </summary>
    public string Group { get; set; }
    public MatchSubscriberByEqualsGroupSpecification(string group)
    {
        Group = group;
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return item => item.Group.Equals(Group);
    }
}