using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Framework.Infrastructure.Crosscutting.Extensions;
using Framework.Repository.MongoDB.Builder;

namespace Framework.Repository.MongoDB.Models
{
    /// <summary>
    /// 实体类型 和 集合 的对应配置信息类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : IEntity

    {
        /// <summary>
        /// </summary>
        public EntityTypeConfiguration()
            : this(typeof(TEntity).Name.Pluralize().ToPascalCase())
        {
        }

        /// <summary>
        /// </summary> 
        public EntityTypeConfiguration(string collectionName)
            : this(collectionName, new List<BsonClassMap<TEntity>>())
        {
        }

        /// <summary>
        /// </summary> 
        public EntityTypeConfiguration(string collectionName, IList<BsonClassMap<TEntity>> bsonClassMaps)
        {
            EntityType = typeof(TEntity);
            CollectionName = collectionName;
            BsonClassMaps = new List<BsonClassMap>();

            if (bsonClassMaps != null && bsonClassMaps.Any())
            {
                foreach (var classMap in bsonClassMaps)
                {
                    BsonClassMaps.Add(classMap);
                }
            }

            IndexDefinitions = new List<CollectionIndexDefinition<TEntity>>();
        }

        /// <summary>
        /// 实体的类型
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 集合名称
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// 是否是一个分片集合
        /// </summary>
        public bool IsShardCollection { get; set; }

        /// <summary>
        /// 实体集合BSON映射
        /// </summary>
        public IList<BsonClassMap> BsonClassMaps { get; set; }

        /// <summary>
        /// 分片集合的键值
        /// </summary>
        public IList<BsonElement> ShardKeys { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public IList<CollectionIndexDefinition<TEntity>> IndexDefinitions { get; set; }
    }
}
