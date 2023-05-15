using Framework.Application.Core.Queries;
using Framework.Domain.Core.Repositories;
using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting;
using Framework.Infrastructure.Crosscutting.Adapter;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using MessageCore.Domain.AggregatesModels.TemplateAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Queries.Impl
{
    public class TemplateQueries : ITemplateQueries
    {
        private readonly IReadOnlyTemplateRepository _templateReadOnlyRepository;
        private readonly ITypeAdapter _typeAdapter;

        public TemplateQueries(IReadOnlyTemplateRepository templateReadOnlyRepository, ITypeAdapter typeAdapter)
        {
            _templateReadOnlyRepository = templateReadOnlyRepository;
            _typeAdapter = typeAdapter;
        }

        public async Task<PagingFindResultWrapper<TemplateVM>> GetTemplateByPageAsync(TemplateQuery query)
        {
            Check.Argument.IsNotNull(query, "TemplateQuery");
            ISpecification<Template>? specification = null;
            if (!string.IsNullOrWhiteSpace(query.TemplateName))
            {
                specification = new MatchTemplateByTemplateNameSpecification<Template>(query.TemplateName.Trim());
            }

            var result = await _templateReadOnlyRepository.GetPagedListAndCountAsync(query.PageNumber, query.PageSize, specification,
               new List<SortCriteria<Template>>() { new SortCriteria<Template>((m) => m.UpdateOn, SortOrder.Descending) });
            var result1 = _typeAdapter.Adapt<PagingFindResultWrapper<TemplateVM>>(result);
            return result1;
        }
    }
}
