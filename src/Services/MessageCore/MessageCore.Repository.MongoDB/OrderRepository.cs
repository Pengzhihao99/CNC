using Framework.Domain.Core.Events;
using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.OrderAggregate;
using MessageCore.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB
{
    public class OrderRepository : MongoDBRepository<Order,string>, IOrderRepository
    {
        public OrderRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider) : base(mongoDbContext, domainEventBus, serviceProvider)
        {
        }
    }
}
