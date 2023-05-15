using Framework.Domain.Core.Entities;
using Framework.Repository.MongoDB.Builder;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.Models
{
    /// <summary>
    /// 定义 实体类型 配置信息
    /// </summary>
    public interface IEntityTypeConfiguration
    {
        /// <summary>
        /// 实体的类型
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 实体对应的Mongodb集合的名称
        /// </summary>
        string CollectionName { get; set; }

        /// <summary>
        /// BsonClassMap集合
        /// 如果存在继承关系的情况下，一般需要多个BsonClassMap
        /// </summary>
        IList<BsonClassMap> BsonClassMaps { get; set; }

        /// <summary>
        /// 是否是一个分片集合
        /// </summary>
        //bool IsShardCollection { get; set; }

        /// <summary>
        /// Mongodb分片集合的键值
        ///
        /// 创建分片前，一般需要有对应的索引，空集合分片可以自动创建索引，有数据的则不行。
        ///  <para>
        ///    (1)hash索引： 只能一个字段， new List&lt;BsonElement&gt;() { new BsonElement("_id", "hashed")  } 
        ///  </para>
        ///  <para>
        ///    (2)range索引： 多个字段，但是必须都是升序， new List&lt;BsonElement&gt;() { new BsonElement("UserCode", 1), new BsonElement("CreateTime", 1)} 
        ///  </para>
        /// </summary>
        //IList<BsonElement> ShardKeys { get; set; }
    }

    public interface IEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration
        where TEntity : IEntity
    {
        /// <summary>
        /// 需要增加的索引，会根据索引名称和数据库中已有的匹配，不存在才创建。
        /// </summary>
        IList<CollectionIndexDefinition<TEntity>> IndexDefinitions { get; set; }
    }
}
