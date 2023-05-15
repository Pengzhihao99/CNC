using Framework.Domain.Core.Entities;
using Framework.Repository.MongoDB;
using Framework.Repository.MongoDB.Configurations;
using Framework.Repository.MongoDB.Models;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using MessageCore.Domain.AggregatesModels.OrderAggregate;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Repository.MongoDB.Managements
{
    public class MessageCoreConfigurationManager : MongoDBConfigurationManager
    {
        public MessageCoreConfigurationManager(ILogger<MongoDBContext> logger, IMongoDBContext context)
            : base(logger, context)
        {
        }
        protected override string CollectionsAssemblyName => typeof(Blocking).Assembly.FullName;

        protected override IEnumerable<Func<BsonClassMap>> BsonClassMaps => new List<Func<BsonClassMap>>()
        {
            //()=>{
            //    var result = new BsonClassMap(typeof(MessageBlocking));
            //    result.AutoMap();
            //    return result;
            //},
        };

        /// <summary>
        /// 索引
        /// </summary>
        protected override IEnumerable<IndexConfiguration> IndexConfigurations => new List<IndexConfiguration>()
        {
            new IndexConfiguration<Blocking>()
            {
                IndexName = "IDX_UpdateOn",
                IndexKeysDefinition = Builders<Blocking>.IndexKeys.Descending(x => x.UpdateOn),
            },
            new IndexConfiguration<Order>()
            {
                IndexName = "IDX_ReferenceNumber",
                IndexKeysDefinition = Builders<Order>.IndexKeys.Ascending(x => x.ReferenceNumber),
            },
           
        };

       
    }
}
