using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{
    public class ReadOnlySendingServiceRepository : ReadOnlyMongoDBRepository<SendingService, string>, IReadOnlySendingServiceRepository
    {
        public ReadOnlySendingServiceRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
