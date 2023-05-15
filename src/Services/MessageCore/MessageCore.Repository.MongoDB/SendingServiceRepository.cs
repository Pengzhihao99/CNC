using Framework.Domain.Core.Events;
using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.SendingServiceAggregate;
using MessageCore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB
{
    public class SendingServiceRepository : MongoDBRepository<SendingService, string>, ISendingServiceRepository
    {
        public SendingServiceRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider) : base(mongoDbContext, domainEventBus, serviceProvider)
        {
        }
    }
}
