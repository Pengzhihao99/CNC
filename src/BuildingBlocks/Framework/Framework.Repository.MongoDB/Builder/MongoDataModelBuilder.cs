using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain.Core.Entities;
using Framework.Infrastructure.Crosscutting;
using Framework.Infrastructure.Crosscutting.Extensions;
using Framework.Repository.MongoDB.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Framework.Repository.MongoDB.Builder
{
    /// <summary>
    /// Mongodb 数据模型构建器
    /// </summary>
    public class MongoDataModelBuilder
    {
        /// <summary>
        /// 实体类配置信息
        /// </summary>
        private readonly ConcurrentDictionary<Type, IEntityTypeConfiguration> _entityTypeConfigurations;

        // <summary>
        /// 用于同步锁
        /// </summary>
        private static readonly object SyncObj = new object();

        /// <summary>
        /// mongo实例信息
        /// </summary>
        private MongoInstanceInfo _mongoInstanceInfo;

        /// <summary>
        /// mongo数据库信息
        /// </summary>
        private MongoDatabaseInfo _databaseInfo;

        /// <summary>
        /// </summary>
        public MongoDataModelBuilder()
        {
            _entityTypeConfigurations = new ConcurrentDictionary<Type, IEntityTypeConfiguration>();
        }

        /// <summary>
        /// 配置实体类型
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="configureAction">配置实体的委托方法</param>
        public virtual MongoDataModelBuilder Entity<TEntity>(Action<IEntityTypeConfiguration<TEntity>> configureAction = null)
            where TEntity : IEntity
        {
            var entityTypeConfiguration = _entityTypeConfigurations.GetOrAdd(typeof(TEntity), (t) => new EntityTypeConfiguration<TEntity>());

            configureAction?.Invoke((EntityTypeConfiguration<TEntity>)entityTypeConfiguration);

            return this;
        }

        /// <summary>
        /// 配置实体类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="configureAction">配置实体的委托方法</param>
        public virtual MongoDataModelBuilder Entity(Type entityType, Action<IEntityTypeConfiguration> configureAction = null)
        {
            Check.Argument.IsNotNull(entityType, nameof(entityType));

            var entityTypeConfiguration = _entityTypeConfigurations.GetOrAdd(entityType, (t) =>
                    (IEntityTypeConfiguration)Activator.CreateInstance(typeof(EntityTypeConfiguration<>).MakeGenericType(entityType))
            );

            configureAction?.Invoke(entityTypeConfiguration);

            return this;
        }

        /// <summary>
        /// 应用实体配置
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public virtual MongoDataModelBuilder ApplyConfiguration<TEntity>(IEntityTypeConfiguration<TEntity> configuration)
            where TEntity : IEntity<string>
        {
            Check.Argument.IsNotNull(configuration, nameof(configuration));

            _entityTypeConfigurations.AddOrUpdate(typeof(TEntity), configuration, (oldkey, oldvalue) => configuration);

            return this;
        }

        /// <summary>
        /// 应用实体配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public virtual MongoDataModelBuilder ApplyConfiguration(IEntityTypeConfiguration configuration)
        {
            Check.Argument.IsNotNull(configuration, nameof(configuration));

            _entityTypeConfigurations.AddOrUpdate(configuration.EntityType, configuration, (t, v) => configuration);

            return this;
        }

        /// <summary>
        /// 获取指定类型（<typeparamref name="TEntity"/>）对应的实体配置信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns></returns>
        public IEntityTypeConfiguration<TEntity> GetEntityTypeConfiguration<TEntity>()
            where TEntity : IEntity
        {
            return (IEntityTypeConfiguration<TEntity>)_entityTypeConfigurations.GetOrDefault(typeof(TEntity));
        }

        /// <summary>
        /// 获取指定类型（<typeparamref name="TEntity"/>）对应的实体配置信息，如果不存在，则创建一个
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns></returns>
        public IEntityTypeConfiguration<TEntity> GetEntityTypeConfigurationOrCreate<TEntity>(IMongoDBContext mongoDbContext)
            where TEntity : IEntity
        {
            var config = GetEntityTypeConfiguration<TEntity>();

            if (config == null)
            {
                lock (SyncObj)
                {
                    config = GetEntityTypeConfiguration<TEntity>();
                    //二阶判断还是空的话，创建和初始化
                    if (config == null)
                    {
                        Entity<TEntity>(x =>
                        {
                            x.BsonClassMaps = new List<BsonClassMap>()
                            {
                                new BsonClassMap<TEntity>(cm => cm.AutoMap())
                            };
                        });

                        config = GetEntityTypeConfiguration<TEntity>();

                        ConfigureEntity(mongoDbContext, config);
                    }
                }
            }

            return config;
        }

        public IEntityTypeConfiguration GetEntityTypeConfiguration(Type entityType)
        {
            Check.Argument.IsNotNull(entityType, nameof(entityType));

            return _entityTypeConfigurations.GetOrDefault(entityType);
        }

        /// <summary>
        /// 获取已经配置的实体配置类型信息
        /// </summary>
        /// <returns></returns>
        public virtual IList<IEntityTypeConfiguration> GetEntityTypeConfigurations()
        {
            return _entityTypeConfigurations.Values.ToList();
        }

        /// <summary>
        /// 构建已知的模型配置信息
        /// </summary>
        protected internal virtual void BuildAndConfigure(IMongoDBContext mongoDbContext)
        {
            //从数据库读取一些配置信息，如 已存在的集合，分片信息, 数据库是否分片，当前链接的是副本集还是Mongos
            _mongoInstanceInfo = GetMongoInstanceInfo(mongoDbContext);

            _databaseInfo = new MongoDatabaseInfo(mongoDbContext.Database.DatabaseNamespace.DatabaseName);

            //读取所有的集合
            var cols = mongoDbContext.Database.ListCollectionNames().ToList();

            foreach (var collectionName in cols)
            {
                _databaseInfo.CollectionNames.Add(collectionName);
            }

            //获取数据库的是否分片的信息
            //GetDatabaseShardingInfo(mongoDbContext);
            //获取分片集合及其对应的分片键
            //GetCollectionShardKeysIfAny(mongoDbContext);

            //判断实体对应的集合是否存在，初始化是否分片集合等
            foreach (var (entityType, configuration) in _entityTypeConfigurations)
            {
                ConfigureEntity(mongoDbContext, configuration);
            }
        }


        /// <summary>
        /// 读取一些基础信息
        /// </summary>
        protected internal virtual MongoInstanceInfo GetMongoInstanceInfo(IMongoDBContext mongoDbContext)
        {
            //isMaster命令 ( db.isMaster() )，可参阅： https://docs.mongodb.com/manual/reference/command/isMaster/
            //不过，从版本4.4.2起已弃用，改用 hello 代替 ( db.runCommand( { hello: 1 } ) ) 。当然目前也是可以调用，未来可能直接被移除。
            //它返回一个描述 mongod 实例角色的文档
            //如果实例是副本集的成员，则返回结果里面将 返回副本集配置和状态的子集，包括实例是否是副本集的主实例。
            //当发送到不是副本集成员的mongod实例时，返回结果里面将返回此信息的子集
            BsonDocument result = null;

            try
            {
                result = mongoDbContext.Database.RunCommand<BsonDocument>(new BsonDocument()
                {
                    { "hello",1 }
                });
            }
            catch (MongoCommandException ex)
            {
                //Mongo如果是4.4.2以下版本，可能没有这个命令，转回用 isMaster
                if (ex.Message.ContainsIgnoreCase("Command hello failed: no such command: 'hello'"))
                {
                    result = mongoDbContext.Database.RunCommand<BsonDocument>(new BsonDocument()
                    {
                        { "isMaster","" }
                    });
                }
                else
                {
                    throw;
                }
            }

            return new MongoInstanceInfo(result);
        }

        /// <summary>
        /// 配置实体
        /// </summary> 
        protected internal void ConfigureEntity(IMongoDBContext mongoDbContext, IEntityTypeConfiguration configuration)
        {
            //先注册类型映射
            if (configuration.BsonClassMaps.NotNullOrEmpty())
            {
                foreach (var classMap in configuration.BsonClassMaps)
                {
                    //注册 BSON 映射
                    //BsonClassMap.RegisterClassMap(classMap);
                }
            }
            else
            {
                //如果BsonClassMaps没有注册，且当前没有注册过，则自动注册一个。
                //if (!BsonClassMap.IsClassMapRegistered(configuration.EntityType))
                //{
                //    var map = new BsonClassMap(configuration.EntityType);
                //    map.AutoMap();
                //    BsonClassMap.RegisterClassMap(map);
                //}
            }

            //判断是否存在，如果指定的集合不存在，则创建一个。
            if (!_databaseInfo.CollectionNames.Contains(configuration.CollectionName))
            {
                //Starting in MongoDB 4.4, you can create collections and indexes inside a transaction. 
                //The transaction must use read concern "local".
                //可查阅相关 https://docs.mongodb.com/manual/reference/read-concern/
                //mongoDbContext.Database.CreateCollection(mongoDbContext.Session, configuration.CollectionName);

                //创建集合
                //如果基于mongoDbContext.Session，有可能会因为开启了事务问题，导致创建失败。事务的 read concern 要基于 "local"
                //如果通过 无 mongoDbContext.Session 创建collection， 另外一方面又用基于mongoDbContext.Session插入document， 在事务提交也会失败。 最好情况下就是在第一次就把相关集合创建完毕。
                mongoDbContext.Database.CreateCollection(configuration.CollectionName);

                _databaseInfo.CollectionNames.Add(configuration.CollectionName);
            }

            //创建索引
            var entityTypeConfiguration = typeof(IEntityTypeConfiguration<>).MakeGenericType(configuration.EntityType);
            //configuration对象是否也是 泛型类型entityTypeConfiguration的 实例对象
            if (entityTypeConfiguration.IsInstanceOfType(configuration))
            {
                //通过反射方法调用  ConfigureIndex<TEntity>
                var methodInfo = typeof(MongoDataModelBuilder).GetMethod(nameof(ConfigureIndex), BindingFlags.NonPublic | BindingFlags.Instance)?
                    .MakeGenericMethod(configuration.EntityType);
                methodInfo?.Invoke(this, new object[] { mongoDbContext, configuration });
            }

            //如果配置的时候，是分片集合，但是分片Key是空的，先置为非分片。
            //configuration.IsShardCollection = configuration.IsShardCollection && (configuration.ShardKeys.NotNullOrEmpty());

            ////如果当前连接的是分片实例，进行一些关于分片的配置操作
            //if (_mongoInstanceInfo.IsShardedInstance)
            //{
            //    //如果访问不了Config数据库里面的集合信息，则以用户配置为准。
            //    if (!_databaseInfo.IsNotAuthorizedAccseeCollectionsCollectionInConfigs)
            //    {
            //        //如果是配置为分片集合, 但是从config中读取不到（数据库集合未分片）
            //        if (configuration.IsShardCollection && !_databaseInfo.CollectionShardKeys.ContainsKey(configuration.CollectionName))
            //        {
            //            //分片集合
            //            ShardCollection(mongoDbContext, configuration.CollectionName, configuration.ShardKeys);

            //        }
            //        //如果源数据集合是分片，获取其分片Key
            //        else if (_databaseInfo.CollectionShardKeys.ContainsKey(configuration.CollectionName))
            //        {
            //            configuration.IsShardCollection = true;
            //            configuration.ShardKeys = _databaseInfo.CollectionShardKeys[configuration.CollectionName];
            //        }
            //    }
            //}
            //else
            //{
            //    //TODO： 未测试一种情况，分片部署，但是直连一个分片数据服务器。

            //    //非分片实例
            //    configuration.IsShardCollection = false;
            //    configuration.ShardKeys = new List<BsonElement>();
            //}
        }

        protected internal virtual void ConfigureIndex<TEntity>(IMongoDBContext mongoDbContext,
            IEntityTypeConfiguration<TEntity> configuration)
            where TEntity : IEntity<string>
        {
            //如果索引的定义为空
            if (configuration.IndexDefinitions == null || !configuration.IndexDefinitions.Any())
            {
                return;
            }

            var indexModelList = new List<CreateIndexModel<TEntity>>();

            foreach (var indexDefinition in configuration.IndexDefinitions)
            {
                if (indexDefinition.IndexKeys != null && indexDefinition.IndexKeys.Any())
                {
                    IndexKeysDefinition<TEntity> indexKeysDefinition = null;

                    foreach (var indexKeys in indexDefinition.IndexKeys)
                    {
                        if (indexKeys.SortDirection == SortDirection.Ascending)
                        {
                            indexKeysDefinition = indexKeysDefinition == null ? Builders<TEntity>.IndexKeys.Ascending(indexKeys.FieldExpression) : indexKeysDefinition.Ascending(indexKeys.FieldExpression);
                        }
                        else
                        {
                            indexKeysDefinition = indexKeysDefinition == null ? Builders<TEntity>.IndexKeys.Descending(indexKeys.FieldExpression) : indexKeysDefinition.Descending(indexKeys.FieldExpression);
                        }
                    }

                    indexModelList.Add(new CreateIndexModel<TEntity>(indexKeysDefinition, indexDefinition.Options));

                    //var indexKeysDefinitionBSON = indexKeysDefinition.Render(BsonSerializer.LookupSerializer<TEntity>(), BsonSerializer.SerializerRegistry);
                    //indexModelList.Add(new CreateIndexModel<BsonDocument>(indexKeysDefinitionBSON, indexDefinition.Options));
                }
            }

            if (indexModelList.Any())
            {
                var collection = mongoDbContext.Database.GetCollection<TEntity>(configuration.CollectionName);
                var allIndexes = collection.Indexes.List().ToList().ToDictionary(idx => idx["name"].AsString);

                foreach (var indexModel in indexModelList)
                {
                    //如果不包含指定的索引名称，则创建。
                    if (indexModel.Options.Name.NotNullOrBlank() && !allIndexes.ContainsKey(indexModel.Options.Name))
                    {
                        //创建索引
                        collection.Indexes.CreateOne(indexModel);
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据库的是否分片的信息
        /// </summary>
        /// <param name="mongoDbContext"></param>
        protected virtual void GetDatabaseShardingInfo(IMongoDBContext mongoDbContext)
        {
            //如果当前非分片部署
            if (!_mongoInstanceInfo.IsShardedInstance)
            {
                _databaseInfo.IsPartitioned = true;

                return;
            }

            //数据库是否分片，以及分片集合，集合键信息，存放在config数据库中。

            var configDatabase = mongoDbContext.Client.GetDatabase("config");

            var databasesCollection = configDatabase.GetCollection<BsonDocument>("databases");

            try
            {
                //数据库名称
                var databaseName = mongoDbContext.Database.DatabaseNamespace.DatabaseName;
                //id过滤，集合databases，文档的id值就是数据库名称
                var idFilterDefinition = Builders<BsonDocument>.Filter.Eq(f => f["_id"], databaseName);
                //查询数据库下的集合
                var databaseList = databasesCollection.Find(idFilterDefinition).ToList();

                if (!databaseList.Any())
                {
                    _databaseInfo.IsPartitioned = false;
                }
                else
                {
                    var dbDocument = databaseList.First();
                    if (dbDocument.Contains("partitioned") && dbDocument["partitioned"].AsBoolean == true)
                    {
                        _databaseInfo.IsPartitioned = true;
                    }
                }
            }
            catch (MongoCommandException ex)
            {
                //如果是无权限访问，则忽略读取。
                if (ex.Message.ContainsIgnoreCase("not authorized"))
                {
                    _databaseInfo.IsNotAuthorizedAccseeDatabaseCollectionInConfigs = true;
                }
                else
                {
                    throw;
                }
            }
        }

        protected virtual void GetCollectionShardKeysIfAny(IMongoDBContext mongoDbContext)
        {
            //如果当前非分片部署
            if (!_mongoInstanceInfo.IsShardedInstance)
            {
                return;
            }

            //分片的信息，都存放在config数据库
            var configDatabase = mongoDbContext.Client.GetDatabase("config");

            var mongoCollection = configDatabase.GetCollection<BsonDocument>("collections");

            //数据库名称
            var databaseName = mongoDbContext.Database.DatabaseNamespace.DatabaseName;
            //config数据库的collections的 每个文档的_id 格式是 {数据库名称}.{集合名称}， 搜索以指定数据库开头的项
            var idFilterDefinition = Builders<BsonDocument>.Filter.Regex(f => f["_id"],
                new BsonRegularExpression($"^{databaseName}\\."));
            var keyExistsFilterDefinition = Builders<BsonDocument>.Filter.Exists(f => f["key"]); //如果没有分片的集合，或者集合被删除，是没有key这个元素的
            var notDroppedFilterDefinition = Builders<BsonDocument>.Filter.Eq(f => f["dropped"], false); //集合没有被删除的

            List<BsonDocument> collectionList;

            try
            {
                //查询数据库下的集合
                collectionList = mongoCollection.Find(Builders<BsonDocument>.Filter.And(idFilterDefinition, keyExistsFilterDefinition, notDroppedFilterDefinition)).ToList();
            }
            catch (MongoCommandException ex)
            {
                //如果是无权限访问，则忽略读取。
                if (ex.Message.ContainsIgnoreCase("not authorized"))
                {
                    _databaseInfo.IsNotAuthorizedAccseeCollectionsCollectionInConfigs = true;
                    return;
                }
                else
                {
                    throw;
                }
            }

            //无分片的集合
            if (!collectionList.Any())
            {
                return;
            }

            var collectionShardKeys = new Dictionary<string, IList<BsonElement>>();

            foreach (var collectionItem in collectionList)
            {
                var keyValue = collectionItem["key"];

                if (keyValue != null)
                {
                    //如 Hash分片： {"_id" : "hashed"} 
                    //   范围分片的键： { "TenantCode" : 1.0, "CreateOperation.Time" : 1.0 }
                    var keyValueBsonDocument = keyValue.AsBsonDocument;

                    //如存在分片键
                    if (keyValueBsonDocument != null && keyValueBsonDocument.ElementCount > 0)
                    {
                        //提取
                        var collectionName = collectionItem["_id"].AsString.Replace($"{databaseName}.", "");
                        collectionShardKeys.Add(collectionName, new List<BsonElement>());

                        //一个BsonElement 就是键值元素
                        foreach (var kp in keyValueBsonDocument.Elements)
                        {
                            //kp.Name : _id
                            //kp.Value : BsonValue  (Hashed)
                            collectionShardKeys[collectionName].Add(kp);
                        }
                    }
                }
            }

            _databaseInfo.CollectionShardKeys = collectionShardKeys;
        }

        /// <summary>
        /// 分片集合
        /// </summary>
        /// <param name="mongoDbContext"></param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="shardKeys">分片键值</param>
        protected virtual void ShardCollection(IMongoDBContext mongoDbContext, string collectionName, IList<BsonElement> shardKeys)
        {
            var adminDb = mongoDbContext.Client.GetDatabase("admin");

            //数据库名称
            var databaseName = mongoDbContext.Database.DatabaseNamespace.DatabaseName;

            //如果数据库不是分片的，尝试启动分片；如果没有权限执行分片，则忽略。
            if (!_databaseInfo.IsPartitioned && !_databaseInfo.IsNotAuthorizedEnableSharding)
            {
                try
                {
                    var shardDbResult = adminDb.RunCommand<BsonDocument>(new BsonDocument() {

                        { "enableSharding",databaseName}
                    });

                    var okValue = shardDbResult["ok"].AsDouble;
                    if (okValue == 1)
                    {
                        _databaseInfo.IsPartitioned = true;
                    }
                }
                catch (MongoCommandException ex)
                {
                    //如果是无权限访问，则忽略读取。
                    if (ex.Message.ContainsIgnoreCase("not authorized"))
                    {
                        _databaseInfo.IsNotAuthorizedEnableSharding = true;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            //如果没有权限执行分片，则忽略
            if (!_databaseInfo.IsNotAuthorizedShardCollection)
            {
                try
                {
                    //分片集合
                    //var commandDict = new Dictionary<string, object>();
                    //commandDict.Add("shardCollection", $"{databaseName}.{collectionName}");
                    //commandDict.Add("key", new Dictionary<string, object>() { { "_id", "hashed" } });

                    //执行集合分片 sh.shardCollection("databaseName.collectionName", { "_id": "hashed"});
                    var shardCollectionCommand = new BsonElement("shardCollection", $"{databaseName}.{collectionName}");
                    var keyCommand = new BsonElement("key", new BsonDocument(shardKeys));

                    var bsonDocumentCommand = new BsonDocument(new List<BsonElement>()
                    {
                        shardCollectionCommand,
                        keyCommand
                    });
                    var command = new BsonDocumentCommand<BsonDocument>(bsonDocumentCommand);
                    var collectionShardResult = adminDb.RunCommand(command);

                    var okValue = collectionShardResult["ok"].AsDouble;
                    if (okValue == 1)
                    {
                        //成功
                        return;
                    }
                }
                catch (MongoCommandException ex)
                {
                    //如果是无权限访问，则忽略读取。
                    if (ex.Message.ContainsIgnoreCase("not authorized"))
                    {
                        _databaseInfo.IsNotAuthorizedShardCollection = true;
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
