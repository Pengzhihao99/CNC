using Framework.Domain.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB.Builder
{
    /// <summary>
    /// Mongodb集合索引的索引键定义
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CollectionIndexKeyDefinition<TEntity>
        where TEntity : IEntity

    {
        public CollectionIndexKeyDefinition(Expression<Func<TEntity, object>> fieldExpression, SortDirection sortDirection)
        {
            FieldExpression = fieldExpression;
            SortDirection = sortDirection;
        }

        /// <summary>
        /// 字段表达式
        /// </summary>
        public Expression<Func<TEntity, object>> FieldExpression { get; private set; }

        /// <summary>
        /// 排序方向
        /// </summary>
        public SortDirection SortDirection { get; private set; }
    }
}
