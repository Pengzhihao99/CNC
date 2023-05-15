using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Repositories
{
    /// <summary>
    /// 代表一个仓储
    /// </summary>
    public interface IRepository
    {

    }

    /// <summary>
    /// 包含了基础读写及其查询的方法定义的仓库接口
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public interface IRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 将聚合根实体对象添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoot">需要添加到仓储的聚合根实体对象。</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量将指定的聚合根实体对象添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoots">需要添加到仓储的聚合根实例列表。</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task AddAsync(IEnumerable<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新聚合根实体对象。
        /// </summary>
        /// <param name="aggregateRoot">需要更新的聚合根。</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoots">需要更新的聚合根实例列表。</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task UpdateAsync(IEnumerable<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken = default);

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoot">需要从仓储中移除的聚合根。</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoots">需要从仓储中移除的聚合根实例列表。</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task RemoveAsync(IList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量将符合条件的聚合对象从仓储中移除。
        /// </summary>
        /// <param name="specification">条件参数。</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task RemoveAsync(Domain.Core.Specification.ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default);

    }

    /// <summary>
    /// 包含了基础读写及其查询的方法定义的仓库接口
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot>, IReadOnlyRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {

    }
}
