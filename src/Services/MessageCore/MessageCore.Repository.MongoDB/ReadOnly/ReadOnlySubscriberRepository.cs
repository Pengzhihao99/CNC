using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.SubscriberAggregate;
using MessageCore.Domain.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.ReadOnly
{
    public class ReadOnlySubscriberRepository : ReadOnlyMongoDBRepository<Subscriber, string>, IReadOnlySubscriberRepository
    {
        public ReadOnlySubscriberRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }
    }
}
