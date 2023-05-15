using System.Linq.Expressions;
using Framework.Domain.Core.Specification;

namespace MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications;

public class MatchSubscriberByEqualsNameSpecification<T> : Specification<T>
    where T : Subscriber
{
    /// <summary>
    /// 订阅者名字
    /// </summary>
    public string Name { get; set; }

    public MatchSubscriberByEqualsNameSpecification(string name)
    {
        Name = name;
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return item => item.Name.Equals(Name);
    }
}