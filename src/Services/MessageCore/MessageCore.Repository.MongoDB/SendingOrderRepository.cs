using Framework.Domain.Core.Events;
using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB
{
    public class SendingOrderRepository : MongoDBRepository<SendingOrder,string>, ISendingOrderRepository
    {
        public SendingOrderRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider) : base(mongoDbContext, domainEventBus, serviceProvider)
        {
        }
    }
}
