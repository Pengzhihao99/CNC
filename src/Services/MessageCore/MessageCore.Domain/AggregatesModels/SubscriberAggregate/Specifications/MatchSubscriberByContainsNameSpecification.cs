using System.Linq.Expressions;
using Framework.Domain.Core.Specification;

namespace MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications;

public class MatchSubscriberByContainsNameSpecification<T> : Specification<T>
    where T : Subscriber
{
    /// <summary>
    /// 订阅者名字
    /// </summary>
    public string Name { get; set; }

    public MatchSubscriberByContainsNameSpecification(string name)
    {
        Name = name;
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return item => item.Name.Contains(Name);
    }
}