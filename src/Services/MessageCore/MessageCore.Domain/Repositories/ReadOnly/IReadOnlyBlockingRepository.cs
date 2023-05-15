using Framework.Domain.Core.Repositories;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Repositories.ReadOnly
{
    public interface IReadOnlyBlockingRepository : IReadOnlyRepository<Blocking, string>
    {
    }
}
