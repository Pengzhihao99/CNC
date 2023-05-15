using Framework.Domain.Core.Repositories;
using MessageCore.Domain.AggregatesModels.OrderFilterAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Repositories.ReadOnly
{
    public interface IReadOnlyOrderFilterRepository : IReadOnlyRepository<OrderFilter, string>
    {
    }
}
