using Framework.Domain.Core.Events;
using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.IssuerAggregate;
using MessageCore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB
{
    public class IssuerRepository : MongoDBRepository<Issuer, string>, IIssuerRepository
    {
        public IssuerRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider)
          : base(mongoDbContext, domainEventBus, serviceProvider)
        {
        }
    }
}
