using Framework.Domain.Core.Events;
using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using MessageCore.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB
{

    public class TemplateRepository : MongoDBRepository<Template, string>, ITemplateRepository
    {
        public TemplateRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider)
          : base(mongoDbContext, domainEventBus, serviceProvider)
        {
        }
    }

}
