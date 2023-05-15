using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain.Core.Entities;
using MongoDB.Driver;

namespace Framework.Repository.MongoDB
{
    /// <summary>
    /// MongoDB 上下文
    /// 包含访问Mongodb数据库的基础对象·
    /// </summary>
    public interface IMongoDBContext
         
    {
        /// <summary>
        /// Mongo Client 对象
        /// The IMongoClient from the official MongoDb driver
        /// </summary>
        IMongoClient Client { get; }

        /// <summary>
        /// MongoDB 数据库对象
        /// The IMongoDatabase from the official Mongodb driver
        /// </summary>
        IMongoDatabase Database { get; }

        /// <summary>
        /// 数据访问会话对象
        /// </summary>
        IClientSessionHandle Session { get; }

        /// <summary>
        ///  开启事务
        /// </summary>
        /// <returns>是否开始事务成功，如果是则返回ture， 如果已经在事务中，则返回false;</returns>
        bool StartTransaction();

        /// <summary>
        /// MongoDB Collection 数据操作对象
        /// </summary> 
        IMongoCollection<TAggregateRoot> GetCollection<TAggregateRoot>() where TAggregateRoot : IAggregateRoot;

        /// <summary>
        /// 获取一个已经配置的 IEntityTypeConfiguration对象，实体集合的配置信息
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根实体类型</typeparam>
        /// <returns>IEntityTypeConfiguration对象</returns>
        //IEntityTypeConfiguration<TAggregateRoot> GetEntityTypeConfiguration<TAggregateRoot>() where TAggregateRoot : IAggregateRoot;

        string GetCollectionName(Type aggregateRoot);
    }
}
