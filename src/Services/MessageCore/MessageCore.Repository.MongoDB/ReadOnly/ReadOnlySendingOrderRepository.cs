using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{
    public class ReadOnlySendingOrderRepository : ReadOnlyMongoDBRepository<SendingOrder, string>, IReadOnlySendingOrderRepository
    {
        public ReadOnlySendingOrderRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
