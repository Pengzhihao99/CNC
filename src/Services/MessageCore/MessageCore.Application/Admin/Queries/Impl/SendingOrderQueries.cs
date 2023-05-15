using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting.Adapter;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.ViewModels;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.AggregatesModels.OrderAggregate.Specifications;
using MessageCore.Domain.AggregatesModels.OrderAggregate;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate.Specifications;
using Framework.Application.Core.Queries;
using Framework.Domain.Core.Repositories;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;

namespace MessageCore.Application.Admin.Queries.Impl
{
    /// <summary>
    /// 消息订单查询接口实现
    /// </summary>
    public class SendingOrderQueries : ISendingOrderQueries
    {
        private readonly IReadOnlySendingOrderRepository _readOnlySendingOrderRepository;
        private readonly ITypeAdapter _typeAdapter;

        public SendingOrderQueries(IReadOnlySendingOrderRepository readOnlySendingOrderRepository, ITypeAdapter typeAdapter)
        {
            _readOnlySendingOrderRepository = readOnlySendingOrderRepository;
            _typeAdapter = typeAdapter;
        }

        public async Task<PagingFindResultWrapper<SendingOrderVM>> GetSendingOrderByIdOrSenderNameAsync(SendingOrderQuery sendingOrderQuery)
        {
            ISpecification<SendingOrder>? specification = null;
            if (sendingOrderQuery.ReferenceNumbers != null && sendingOrderQuery.ReferenceNumbers.Any())
            {
                specification = new MatchSendingOrderByReferenceNumbersSpecification<SendingOrder>(sendingOrderQuery.ReferenceNumbers.Split(",").ToList());
            }

            if (!string.IsNullOrWhiteSpace(sendingOrderQuery.SenderName))
            {
                if (specification != null)
                {
                    specification = specification.And(new MatchSendingOrderBySenderNameSpecification<SendingOrder>(sendingOrderQuery.SenderName.Trim()));
                }
                else
                {
                    specification = new MatchSendingOrderBySenderNameSpecification<SendingOrder>(sendingOrderQuery.SenderName.Trim());
                }
            }

            var result = await _readOnlySendingOrderRepository.GetPagedListAndCountAsync(sendingOrderQuery.PageNumber, sendingOrderQuery.PageSize, specification,
                  new List<SortCriteria<SendingOrder>>() { new SortCriteria<SendingOrder>((item => item.UpdateOn), SortOrder.Descending) });

            return _typeAdapter.Adapt<PagingFindResultWrapper<SendingOrderVM>>(result);
        }
    }
}
