using Framework.Domain.Core.Repositories;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Repositories.ReadOnly
{
    public interface IReadOnlySendingOrderRepository : IReadOnlyRepository<SendingOrder, string>
    {
    }
}
