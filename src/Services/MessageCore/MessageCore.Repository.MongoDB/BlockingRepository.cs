using Framework.Domain.Core.Events;
using Framework.Repository.MongoDB;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using MessageCore.Domain.Repositories;


namespace MessageCore.Repository.MongoDB
{
    public class BlockingRepository : MongoDBRepository<Blocking,string>, IBlockingRepository
    {
        /// <summary>
        /// 考虑IServiceProvider调整
        /// </summary>
        /// <param name="mongoDbContext"></param>
        /// <param name="domainEventBus"></param>
        /// <param name="serviceProvider"></param>
        public BlockingRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider)
           : base(mongoDbContext, domainEventBus, serviceProvider)
        {
        }
    }
}