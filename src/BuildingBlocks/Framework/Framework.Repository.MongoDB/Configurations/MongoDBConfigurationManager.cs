using Framework.Domain.Core.Entities;
using Framework.Repository.MongoDB.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.Configurations
{
    public abstract class MongoDBConfigurationManager
    {
        public MongoDBConfigurationManager(ILogger<MongoDBContext> logger, IMongoDBContext context)
        {
            _context = context;
            _logger = logger;
        }

        private readonly IMongoDBContext _context;
        private readonly ILogger<MongoDBContext> _logger;

        /// <summary>
        /// 数据迁移
        /// </summary>
        public virtual void Migration()
        {
            try
            {
                _logger.LogInformation($"Migrating database start");

                var retry = Policy.Handle<System.Exception>()
                        .WaitAndRetry(new TimeSpan[]
                        {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(8),
                        });

                retry.Execute(() =>
                {
                    //创建集合
                    CreateCollections();
                    _logger.LogInformation($"CreateCollections complete");
                });

                //创建Index
                CreateIndexs();
                _logger.LogInformation($"GenerateIndex complete");

                _logger.LogInformation($"Migrated database success");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while migrating the database");
            }
        }

        /// <summary>
        /// 数据库配置
        /// </summary>
        public virtual void ConfigDatabase()
        {
            try
            {
                _logger.LogInformation($"Config database start");

                RegisterConventions(null);
                _logger.LogInformation($"RegisterConventions complete");

                RegisterSerializer();
                _logger.LogInformation($"RegisterSerializer complete");

                RegisterBsonClassMap();
                _logger.LogInformation($"RegisterBsonClassMap complete");

                _logger.LogInformation($"Config database success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while config the database");
            }
        }

        /// <summary>
        /// 映射
        /// </summary>
        protected abstract IEnumerable<Func<BsonClassMap>> BsonClassMaps { get; }

        /// <summary>
        /// 索引
        /// </summary>
        protected abstract IEnumerable<IndexConfiguration> IndexConfigurations { get; }

        /// <summary>
        /// 集合程序集名字
        /// </summary>
        protected abstract string CollectionsAssemblyName { get; }

        /// <summary>
        /// 注册转换器
        /// </summary>
        /// <param name="additionConventions"></param>
        protected virtual void RegisterConventions(IEnumerable<IConvention> additionalConventions)
        {
            var conventionPack = new ConventionPack
            {
                //注册转化器
                new NamedIdMemberConvention("id", "_id"),
                new IgnoreExtraElementsConvention(true),
                new StringObjectIdIdGeneratorConvention(),
                //枚举序列化为字符串
                new EnumRepresentationConvention(BsonType.String)
            };

            if (additionalConventions != null)
            {
                conventionPack.AddRange(additionalConventions);
            }

            ConventionRegistry.Register("DefaultConvention", conventionPack, t => true);
        }

        /// <summary>
        /// 注册序列化
        /// </summary>
        protected virtual void RegisterSerializer()
        {
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
        }

        /// <summary>
        /// 创建表结构
        /// </summary>
        protected virtual void CreateCollections()
        {
            var collectionNames = GetAllCollectionNames(new List<string>() { CollectionsAssemblyName });

            if (collectionNames != null && collectionNames.Count() > 0)
            {
                var dataBaseCollectionNames = _context.Database.ListCollectionNames().ToList();

                foreach (var collectionName in collectionNames)
                {
                    if (!dataBaseCollectionNames.Any(item => item == collectionName))
                    {
                        _context.Database.CreateCollection(collectionName);
                    }
                }
            }
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        protected virtual void CreateIndexs()
        {
            if (IndexConfigurations != null && IndexConfigurations.Count() > 0)
            {
                foreach (var indexConfiguration in IndexConfigurations)
                {
                    var entityTypeConfiguration = typeof(IndexConfiguration<>).MakeGenericType(indexConfiguration.EntityType);
                    if (entityTypeConfiguration.IsInstanceOfType(indexConfiguration))
                    {
                        //通过反射方法调用
                        var methodInfo = typeof(MongoDBConfigurationManager).GetMethod(nameof(CreateIndex), BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(indexConfiguration.EntityType);
                        methodInfo?.Invoke(this, new object[] { indexConfiguration });
                    }
                }
            }
        }

        /// <summary>
        /// 映射
        /// </summary>
        protected virtual void RegisterBsonClassMap()
        {
            if (BsonClassMaps != null && BsonClassMaps.Count() > 0)
            {
                foreach (var bsonClassMap in BsonClassMaps)
                {
                    var bsonMap = bsonClassMap();
                    if (!BsonClassMap.IsClassMapRegistered(bsonMap.ClassType))
                    {
                        BsonClassMap.RegisterClassMap(bsonMap);
                    }
                }
            }
        }

        #region private

        protected void CreateIndex<TAggregateRoot>(IndexConfiguration<TAggregateRoot> indexConfiguration)
            where TAggregateRoot : IAggregateRoot
        {
            var collections = _context.GetCollection<TAggregateRoot>();
            using (var cursor = collections.Indexes.List())
            {
                var indexNames = GetIndexNames(cursor.ToEnumerable());
                if (!indexNames.Any(item => item == indexConfiguration.IndexName))
                {
                    if (indexConfiguration.CreateIndexOptions != null)
                    {
                        indexConfiguration.CreateIndexOptions.Name = indexConfiguration.IndexName;
                        collections.Indexes.CreateOne(new CreateIndexModel<TAggregateRoot>(indexConfiguration.IndexKeysDefinition, indexConfiguration.CreateIndexOptions));
                    }
                    else
                    {
                        collections.Indexes.CreateOne(new CreateIndexModel<TAggregateRoot>(indexConfiguration.IndexKeysDefinition, new CreateIndexOptions() { Name = indexConfiguration.IndexName }));
                    }
                }
            }
        }

        private List<string> GetIndexNames(IEnumerable<BsonDocument> bsonDocuments)
        {
            return bsonDocuments.Select(item => item.GetValue("name").ToString()).Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
        }

        private List<string> GetAllCollectionNames(List<string> assemblyNames)
        {
            List<string> names = new List<string>();
            List<Assembly> assList = new List<Assembly>();

            if (assemblyNames == null || assemblyNames.Count == 0)
            {
                assList = AppDomain.CurrentDomain.GetAssemblies().ToList();
            }
            else
            {
                foreach (var assemblyName in assemblyNames)
                {
                    var ass = Assembly.Load(assemblyName);
                    if (ass != null)
                    {
                        assList.Add(ass);
                    }
                }
            }

            foreach (var ass in assList)
            {
                var aggs = ass.GetTypes()
                    .Where(c => c.IsPublic && c.IsClass && !c.IsAbstract && c.GetInterfaces().Contains(typeof(IAggregateRoot))).ToList();
                if (aggs.Any())
                {
                    foreach (var item in aggs)
                    {
                        var name = _context.GetCollectionName(item.GetTypeInfo());
                        names.Add(name);
                    }
                }
            }

            return names;
        }

        #endregion
    }
}
