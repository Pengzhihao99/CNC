using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Domain.Core.Attributes;
using Framework.Domain.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Framework.Infrastructure.Crosscutting.Extensions;
using Framework.Repository.MongoDB.Builder;
using Framework.Repository.MongoDB.Models;

namespace Framework.Repository.MongoDB
{
    public class MongoDBContext : IMongoDBContext
    {
        /// <summary>
        /// MongoDBContext Logger
        /// </summary>
        private readonly ILogger<MongoDBContext> _logger;

        /// <summary>
        /// 连接配置
        /// </summary>
        private readonly MongoDBConnectionOptions _connectionOptions;

        /// <summary>
        /// Mongo Client 对象
        /// The IMongoClient from the official MongoDb driver
        /// </summary>
        public IMongoClient Client { get; private set; }

        /// <summary>
        /// MongoDB 数据库对象
        /// The IMongoDatabase from the official Mongodb driver
        /// </summary>
        public IMongoDatabase Database { get; private set; }

        /// <summary>
        /// 数据访问会话对象
        /// </summary>
        public IClientSessionHandle Session { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public MongoDBContext(ILogger<MongoDBContext> logger, IOptions<MongoDBConnectionOptions> options)
        {
            _logger = logger;
            _connectionOptions = options.Value;

            //创建客户端
            Client = new MongoClient(_connectionOptions.ConnectionString);
            Database = Client.GetDatabase(_connectionOptions.Database);

            //Session = Client.StartSession();
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public virtual bool StartTransaction()
        {
            if (Session == null)
            {
                Session = Client.StartSession();
            }

            if (!Session.IsInTransaction)
            {
                //开启事务
                Session.StartTransaction(new TransactionOptions(ReadConcern.Majority,
                    ReadPreference.Primary, WriteConcern.WMajority));

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取IMongoCollection对象
        /// </summary>
        public IMongoCollection<TAggregateRoot> GetCollection<TAggregateRoot>()
            where TAggregateRoot : IAggregateRoot
        {
            //var collectionConfiguration = GetEntityTypeConfigurationOrCreate<TAggregateRoot>();
            return Database.GetCollection<TAggregateRoot>(GetCollectionName(typeof(TAggregateRoot)));
        }

        /// <summary>
        /// 获取集合名称
        /// </summary>
        /// <param name="type">集合类型</param>
        public string GetCollectionName(Type aggregateRoot)
        {
            var type = aggregateRoot;

            if (_collectionNameDictionary.ContainsKey(type))
            {
                return _collectionNameDictionary[type];
            }
            else
            {
                lock (_lockForCollectionNameDictionary)
                {
                    if (!_collectionNameDictionary.ContainsKey(type))
                    {
                        var collectionName = GetCollectionNameFromAttribute(type) ?? Pluralize(type);

                        _collectionNameDictionary[type] = collectionName;

                        return collectionName;
                    }

                    return _collectionNameDictionary[type];
                }
            }
        }

        #region private

        /// <summary>
        /// 存储集合名称的字典
        /// </summary>
        private static Dictionary<Type, string> _collectionNameDictionary = new Dictionary<Type, string>();
        private static readonly object _lockForCollectionNameDictionary = new object();

        /// <summary>
        /// 返回集合名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="isSplitedCollection">true:分表 false:未分表</param>
        /// <returns></returns>
        private string GetCollectionNameFromAttribute(Type t)
        {
            var name = (t.GetTypeInfo().GetCustomAttributes(typeof(AggregateRootNameAttribute)).FirstOrDefault() as AggregateRootNameAttribute)?.Name;
            return name;
        }

        private string Pluralize(Type type)
        {
            return (type.Name.Pluralize()).ToCamelCase();
        }

        #endregion

        #region MyRegion

        /// <summary>
        /// 同步锁对象
        /// </summary>
        private static readonly object SyncObj = new object();

        /// <summary>
        /// MongoDB数据模型构建器
        /// </summary>
        protected static MongoDataModelBuilder MongoDataModelBuilder;

        /// <summary>
        /// MongoDB数据序列化构建器
        /// </summary>
        protected static MongoSerializationConfigBuilder MongoSerializationConfigBuilder;


        /// <summary>
        /// 获取一个已经配置的 IEntityTypeConfiguration对象，实体集合的配置信息。 若不存在则返回空
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体类型</typeparam>
        /// <returns>IEntityTypeConfiguration对象</returns>
        public IEntityTypeConfiguration<TAggregateRoot> GetEntityTypeConfiguration<TAggregateRoot>()
            where TAggregateRoot : IAggregateRoot
        {
            return GetMongoDataModelBuilderOrCreate().GetEntityTypeConfiguration<TAggregateRoot>();
        }

        /// <summary>
        /// 配置MongoDB数据模型, 方法默认空实现。
        /// 重写此方法以进一步配置根据实体类型的约定构建数据模型
        /// </summary>
        /// <param name="builder">Mongodb数据模型构建器</param>
        protected internal virtual void ConfigureDataModel(MongoDataModelBuilder builder)
        {
        }

        /// <summary>
        /// 配置序列化配置信息。默认配置了:
        /// <para>
        ///     基础的转化器, 有 Mongodb文档_id和字段id的映射转换，忽略不存在的元素，字符串和文档ObjectId的转换 以及 枚举序列化为字符串
        /// </para>
        /// <para>
        ///     序列化器，有decimal类型的序列号器<see cref="DecimalSerializer"/>, <see cref="NullableSerializer{T}"/>
        /// </para>
        /// 重写此方法以进一步配置各种自定义序列化转化配置。
        /// </summary>
        /// <param name="builder">Mongo 序列化配置信息 构建器</param>
        protected internal virtual void RegisterSerializationConfig(MongoSerializationConfigBuilder builder)
        {
            //注册转化器
            builder.RegisterConvention(new NamedIdMemberConvention("id", "_id"));
            builder.RegisterConvention(new IgnoreExtraElementsConvention(true));
            builder.RegisterConvention(new StringObjectIdIdGeneratorConvention());
            //枚举序列化为字符串
            builder.RegisterConvention(new EnumRepresentationConvention(BsonType.String));

            //注册序列化器
            builder.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            builder.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
        }

        /// <summary>
        /// 获取IEntityTypeConfiguration对象，实体集合的配置信息
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体类型</typeparam>
        /// <returns></returns>
        protected virtual IEntityTypeConfiguration<TAggregateRoot> GetEntityTypeConfigurationOrCreate<TAggregateRoot>()
            where TAggregateRoot : IAggregateRoot
        {
            //获取自定义配置的 或者已经系统创建的对应类型的 IEntityTypeConfiguration
            //如果没有，则默认创建一个对应类型的

            var builder = GetMongoDataModelBuilderOrCreate();

            return builder.GetEntityTypeConfigurationOrCreate<TAggregateRoot>(this);
        }

        /// <summary>
        /// 获取模型构造器，如果不存在，则创建一个
        /// </summary>
        /// <returns></returns>
        protected virtual MongoDataModelBuilder GetMongoDataModelBuilderOrCreate()
        {
            //先初始化基础注册
            if (MongoSerializationConfigBuilder == null)
            {
                lock (SyncObj)
                {
                    //双重判断避免重复执行
                    if (MongoSerializationConfigBuilder == null)
                    {
                        var mongoSerializationConfigBuilder = new MongoSerializationConfigBuilder();
                        RegisterSerializationConfig(mongoSerializationConfigBuilder);
                        //构建配置

                        //注册Convention(ConventionRegistry)
                        mongoSerializationConfigBuilder.BuildAndConfigure();

                        MongoSerializationConfigBuilder = mongoSerializationConfigBuilder;
                    }
                }
            }

            if (MongoDataModelBuilder == null)
            {
                lock (SyncObj)
                {
                    //双重判断避免重复执行
                    if (MongoDataModelBuilder == null)
                    {
                        //首次访问仓储的时候，读取Config和创建相关的配置
                        var mongoDataModelBuilder = new MongoDataModelBuilder();
                        ConfigureDataModel(mongoDataModelBuilder);
                        //构建配置


                        //RegisterClassMap
                        //CreateCollection
                        //CreateIndexModel
                        mongoDataModelBuilder.BuildAndConfigure(this);

                        //赋值于实例上
                        MongoDataModelBuilder = mongoDataModelBuilder;
                    }
                }
            }

            return MongoDataModelBuilder;
        }

        #endregion

    }
}