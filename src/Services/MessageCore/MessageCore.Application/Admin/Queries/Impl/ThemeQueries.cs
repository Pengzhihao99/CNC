using Framework.Application.Core.Queries;
using Framework.Domain.Core.Repositories;
using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting;
using Framework.Infrastructure.Crosscutting.Adapter;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Domain.AggregatesModels.ThemeAggregate;
using MessageCore.Domain.AggregatesModels.ThemeAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Queries.Impl
{
    public class ThemeQueries : IThemeQueries
    {
        private readonly IReadOnlyThemeRepository _themeReadOnlyRepository;
        private readonly ITypeAdapter _typeAdapter;

        public ThemeQueries(IReadOnlyThemeRepository themeReadOnlyRepository, ITypeAdapter typeAdapter)
        {
            _themeReadOnlyRepository = themeReadOnlyRepository;
            _typeAdapter = typeAdapter;
        }

        public async Task<PagingFindResultWrapper<ThemeVM>> GetThemeByPageAsync(ThemeQuery query)
        {
            Check.Argument.IsNotNull(query, "ThemeQuery");
            ISpecification<Theme>? specification = null;
            if (!string.IsNullOrWhiteSpace(query.Id))
            {
                specification = new MatchThemeByIdSpecification<Theme>(query.Id);
            }
            var result = await _themeReadOnlyRepository.GetPagedListAndCountAsync(query.PageNumber, query.PageSize, specification,
               new List<SortCriteria<Theme>>() { new SortCriteria<Theme>((m) => m.UpdateOn, SortOrder.Descending) });
            var result1 = _typeAdapter.Adapt<PagingFindResultWrapper<ThemeVM>>(result);
            return result1;
        }
    }
}
