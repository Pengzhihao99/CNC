using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using MessageCore.Domain.Repositories.ReadOnly;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{
    public class ReadOnlyBlockingRepository : ReadOnlyMongoDBRepository<Blocking, string>, IReadOnlyBlockingRepository
    {
        public ReadOnlyBlockingRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
