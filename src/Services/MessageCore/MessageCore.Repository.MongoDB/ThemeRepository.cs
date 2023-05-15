using Framework.Domain.Core.Events;
using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.TemplateAggregate;
using MessageCore.Domain.AggregatesModels.ThemeAggregate;
using MessageCore.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB
{

    public class ThemeRepository : MongoDBRepository<Theme, string>, IThemeRepository
    {
        public ThemeRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider)
          : base(mongoDbContext, domainEventBus, serviceProvider)
        {
        }
    }

}
