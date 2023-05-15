using Framework.Application.Core.Queries;
using Framework.Domain.Core.Repositories;
using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting;
using Framework.Infrastructure.Crosscutting.Adapter;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;

namespace MessageCore.Application.Admin.Queries.Impl;

public class SubscriberQueries:ISubscriberQueries
{
    private readonly IReadOnlySubscriberRepository _subscriberReadOnlyRepository;
    private readonly ITypeAdapter _typeAdapter;

    public SubscriberQueries(IReadOnlySubscriberRepository subscriberReadOnlyRepository, ITypeAdapter typeAdapter)
    {
        _subscriberReadOnlyRepository = subscriberReadOnlyRepository;
        _typeAdapter = typeAdapter;
    }

    public async Task<PagingFindResultWrapper<SubscriberVM>> GetSubscribersByPageAsync(SubscriberQuery query)
    {
        Check.Argument.IsNotNull(query, "SubscriberQuery");
        ISpecification<Subscriber>? specification = null;
        if (!string.IsNullOrWhiteSpace(query.Group))
        {
            specification = new MatchSubscriberByGroupSpecification<Subscriber>(query.Group);
        }

        var result = await _subscriberReadOnlyRepository.GetPagedListAndCountAsync(query.PageNumber, query.PageSize, specification,
            new List<SortCriteria<Subscriber>>() { new SortCriteria<Subscriber>((m) => m.UpdateOn, SortOrder.Descending) });
        
        return _typeAdapter.Adapt<PagingFindResultWrapper<SubscriberVM>>(result);
    }
}