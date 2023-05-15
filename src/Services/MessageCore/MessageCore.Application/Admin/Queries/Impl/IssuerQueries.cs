using Framework.Application.Core.Queries;
using Framework.Domain.Core.Repositories;
using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting;
using Framework.Infrastructure.Crosscutting.Adapter;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Domain.AggregatesModels.IssuerAggregate;
using MessageCore.Domain.AggregatesModels.IssuerAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Queries.Impl
{
    public class IssuerQueries : IIssuerQueries
    {
        private readonly IReadOnlyIssuerRepository _senderReadOnlyRepository;
        private readonly ITypeAdapter _typeAdapter;

        public IssuerQueries(IReadOnlyIssuerRepository senderReadOnlyRepository, ITypeAdapter typeAdapter)
        {
            _senderReadOnlyRepository = senderReadOnlyRepository;
            _typeAdapter = typeAdapter;
        }

        public Task<PagingFindResultWrapper<IssuerVM>> GetIssuerByNameOrStatusAsync(string SenderName, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingFindResultWrapper<IssuerVM>> GetIssuerByPageAsync(IssuerQuery query)
        {
            Check.Argument.IsNotNull(query, "IssuerQuery");
            //Check.Argument.IsNotNullOrEmpty(query.Id, "MessageSenderQuery.Id");
            ISpecification<Issuer>? specification = null;
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                specification = new MatchIssuerByNameSpecification<Issuer>(query.Name);
            }

            var result = await _senderReadOnlyRepository.GetPagedListAndCountAsync(query.PageNumber, query.PageSize, specification,
               new List<SortCriteria<Issuer>>() { new SortCriteria<Issuer>((m) => m.UpdateOn, SortOrder.Descending) });
            return _typeAdapter.Adapt<PagingFindResultWrapper<IssuerVM>>(result);

        }
    }
}
