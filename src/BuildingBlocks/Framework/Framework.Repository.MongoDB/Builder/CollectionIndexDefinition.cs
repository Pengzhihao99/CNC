using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Framework.Repository.MongoDB.Builder
{
    /// <summary>
    /// Mongodb集合索引的定义
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public class CollectionIndexDefinition<TEntity>
        where TEntity : IEntity
    {
        public CollectionIndexDefinition()
        {
            IndexKeys = new List<CollectionIndexKeyDefinition<TEntity>>();
        }

        public IList<CollectionIndexKeyDefinition<TEntity>> IndexKeys { get; internal set; }

        /// <summary>
        /// 创建Index的相关额外选项
        /// </summary>
        public CreateIndexOptions Options { get; internal set; }

        /// <summary>
        /// 按字段升序
        /// </summary>
        public CollectionIndexDefinition<TEntity> Ascending(Expression<Func<TEntity, object>> field)
        {
            IndexKeys.Add(new CollectionIndexKeyDefinition<TEntity>(field, SortDirection.Ascending));

            return this;
        }

        /// <summary>
        /// 按字段降序
        /// </summary> 
        public CollectionIndexDefinition<TEntity> Descending(Expression<Func<TEntity, object>> field)
        {
            IndexKeys.Add(new CollectionIndexKeyDefinition<TEntity>(field, SortDirection.Descending));

            return this;
        }
    }
}
