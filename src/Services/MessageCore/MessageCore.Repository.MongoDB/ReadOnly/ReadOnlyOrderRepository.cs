using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.OrderAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{
    public class ReadOnlyOrderRepository : ReadOnlyMongoDBRepository<Order, string>, IReadOnlyOrderRepository
    {
        public ReadOnlyOrderRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
