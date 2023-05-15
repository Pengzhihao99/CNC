using Framework.Application.Core.Queries;
using Framework.Domain.Core.Repositories;
using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting.Adapter;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate.Specifications;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Admin.Queries.Impl
{
    public class SendingServiceQueries : ISendingServiceQueries
    {
        private readonly IReadOnlySendingServiceRepository _readOnlySendingServiceRepository;
        private readonly ITypeAdapter _typeAdapter;

        public SendingServiceQueries(IReadOnlySendingServiceRepository readOnlySendingServiceRepository, ITypeAdapter typeAdapter)
        {
            _readOnlySendingServiceRepository = readOnlySendingServiceRepository;
            _typeAdapter = typeAdapter;
        }

        public async Task<PagingFindResultWrapper<SendingServiceVM>> GetSendingServiceByPageAsync(SendingServiceQuery query)
        {
            ISpecification<SendingService>? specification = null;
            if (!string.IsNullOrWhiteSpace(query.ServiceName))
            {
                specification = new MatchSendingServiceByServiceNameSpecification<SendingService>(query.ServiceName.Trim());
            }

            var result = await _readOnlySendingServiceRepository.GetPagedListAndCountAsync(query.PageNumber, query.PageSize, specification,
               new List<SortCriteria<SendingService>>() { new SortCriteria<SendingService>((m) => m.UpdateOn, SortOrder.Descending) });
            return _typeAdapter.Adapt<PagingFindResultWrapper<SendingServiceVM>>(result);
        }

        public async Task<List<SendingServiceNamesVM>> GetSendingServiceNameAllAsync()
        {
            var result = await _readOnlySendingServiceRepository.GetListAsync(new MatchSendingServiceByEnabledSpecification<SendingService>(true));
            //var result = sendingServices.Select(item => new SendingServiceNamesVM()
            //{
            //    Id = item.Id, 
            //    ServiceName = item.ServiceName, 
            //    SendingServiceType = item.SendingServiceType,
            //});
            return _typeAdapter.Adapt<List<SendingServiceNamesVM>>(result);
        }
    }
}
