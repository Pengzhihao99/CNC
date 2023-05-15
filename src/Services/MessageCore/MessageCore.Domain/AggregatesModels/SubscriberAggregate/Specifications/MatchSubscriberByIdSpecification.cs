using System.Linq.Expressions;
using Framework.Domain.Core.Specification;

namespace MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications;

public class MatchSubscriberByIdSpecification<T> : Specification<T>
    where T : Subscriber
{
    public string Id { get; private set; }
    public MatchSubscriberByIdSpecification(string id)
    {
        Id = id;
    }

    public override Expression<Func<T, bool>> GetExpression()
    {
        return item => item.Id.Equals(Id);
    }
}