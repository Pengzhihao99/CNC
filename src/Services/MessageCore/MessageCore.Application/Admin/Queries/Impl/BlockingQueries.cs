using Framework.Application.Core.Queries;
using Framework.Domain.Core.Repositories;
using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting;
using Framework.Infrastructure.Crosscutting.Adapter;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Application.Base;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using MessageCore.Domain.AggregatesModels.BlockingAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Queries.Impl
{
    public class BlockingQueries : IBlockingQueries
    {
        private readonly IReadOnlyBlockingRepository _blockingReadOnlyRepository;
        private readonly ITypeAdapter _typeAdapter;

        public BlockingQueries(IReadOnlyBlockingRepository blockingReadOnlyRepository, ITypeAdapter typeAdapter)
        {
            _blockingReadOnlyRepository = blockingReadOnlyRepository;
            _typeAdapter = typeAdapter;
        }

        public async Task<BlockingVM> GetBlockingByIdAsync(string id)
        {
            Check.Argument.IsNotNullOrEmpty(id, "id");
            var result = await _blockingReadOnlyRepository.GetByKeyAsync(id);

            //测试用
            var result2 = await _blockingReadOnlyRepository.GetListAsync(new MatchBlockingByIdBatchSpecification<Blocking>(new List<string>() { id }));

            var result3 = await _blockingReadOnlyRepository.GetListAsync();

            var result5 = result3.FirstOrDefault();

            var result4 = _typeAdapter.Adapt<BlockingVM>(result);
            return result4;
        }

        public async Task<PagingFindResultWrapper<BlockingVM>> GetBlockingByPageAsync(BlockingQuery query)
        {
            Check.Argument.IsNotNull(query, "BlockingQuery");

            ISpecification<Blocking> specification = null;
            if (!string.IsNullOrWhiteSpace(query.TemplateName))
            {
                specification = new MatchBlockingByTemplateNameSpecification<Blocking>(query.TemplateName.Trim());
            }

            var result = await _blockingReadOnlyRepository.GetPagedListAndCountAsync(query.PageNumber, query.PageSize, specification,
                  new List<SortCriteria<Blocking>>() { new SortCriteria<Blocking>((item => item.UpdateOn), SortOrder.Descending) });

            return _typeAdapter.Adapt<PagingFindResultWrapper<BlockingVM>>(result);
        }
    }
}
