using Framework.Domain.Core.Entities;
using Framework.Domain.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Repositories
{
    /// <summary>
    /// 只读仓储接口
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public interface IReadOnlyRepository<TAggregateRoot> : IRepository
        where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 通过条件获取一个聚合根实体对象，若匹配多个，则返回第一个。
        /// 如果对象不存在，返回null <see cref="EntityNotFoundException"/>
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实例</returns>
        Task<TAggregateRoot?> GetAsync(ISpecification<TAggregateRoot>? specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过条件获取一个聚合根实体对象，若匹配多个，则返回第一个。 如果对象不存在，返回NULL
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实例</returns>
        Task<TAggregateRoot> FindAsync(ISpecification<TAggregateRoot>? specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取所有聚合根实体对象的列表
        /// </summary>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实体列表</returns>
        Task<IList<TAggregateRoot>> GetListAsync(IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取所有聚合根实体对象, 返回的其【投影】对象的列表
        /// </summary>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实体投影实例的列表</returns>
        Task<IList<TProjection>> GetListAsync<TProjection>(IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default) where TProjection : class;

        /// <summary>
        /// 根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体数据.
        /// 若指定了最大返回数量（参数 <paramref name="maxResultCount"/>）, 则最大返回指定数量的结果集。 如目标数据的数量有100个，但是指定只返回前20个。
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="maxResultCount">获取的最大数量限制，默认为0，表示不限制</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实体列表</returns>
        Task<IList<TAggregateRoot>> GetListAsync(ISpecification<TAggregateRoot>? specification, IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, int maxResultCount = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体, 返回的其【投影】对象的列表
        /// 若指定了最大返回数量（参数 <paramref name="maxResultCount"/>）, 则最大返回指定数量的结果集。 如目标数据的数量有100个，但是指定只返回前20个。
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="maxResultCount">获取的最大数量限制，默认为0，表示不限制</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实体列表</returns>
        Task<IList<TProjection>> GetListAsync<TProjection>(ISpecification<TAggregateRoot>? specification, IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, int maxResultCount = 0, CancellationToken cancellationToken = default) where TProjection : class;

        /// <summary>
        /// 分页查询数据，根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体数据。
        /// 根据指定的第几页（<paramref name="pageNumber"/>）和每一页数量（<paramref name="pageSize"/>），返回匹配区间的聚合根实体对象列表数据。
        /// 该方法不会同时计算总数。
        /// </summary>
        /// <param name="pageNumber">页码，第几页</param>
        /// <param name="pageSize">页数据大小，每一页多少条数据。</param>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数据分页结果</returns>
        Task<PagedResult<TAggregateRoot>> GetPagedListAsync(int pageNumber, int pageSize, ISpecification<TAggregateRoot>? specification,
            IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页查询数据，根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体数据。
        /// 根据指定的第几页（<paramref name="pageNumber"/>）和每一页数量（<paramref name="pageSize"/>），返回匹配区间的聚合根实体的【投影】对象列表数据。
        /// 该方法不会同时计算总数。
        /// </summary>
        /// <typeparam name="TProjection">投影类的类型</typeparam>
        /// <param name="pageNumber">页码，第几页</param>
        /// <param name="pageSize">页数据大小，每一页多少条数据。</param>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数据分页结果</returns>
        Task<PagedResult<TProjection>> GetPagedListAsync<TProjection>(int pageNumber, int pageSize, ISpecification<TAggregateRoot>? specification,
            IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default) where TProjection : class;

        /// <summary>
        /// 分页查询数据，根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体数据，同时计算数据的总数。根据指定的第几页（<paramref name="pageNumber"/>）和每一页数量（<paramref name="pageSize"/>），返回匹配区间的聚合根实体对象列表数据。
        /// </summary>
        /// <param name="pageNumber">页码，第几页</param>
        /// <param name="pageSize">页数据大小，每一页多少条数据。</param>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数据分页结果</returns>
        Task<PagedResultWithCount<TAggregateRoot>> GetPagedListAndCountAsync(int pageNumber, int pageSize, ISpecification<TAggregateRoot>? specification,
            IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页查询数据，根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体数据，同时计算数据的总数。根据指定的第几页（<paramref name="pageNumber"/>）和每一页数量（<paramref name="pageSize"/>），返回匹配区间的聚合根实体的【投影】对象列表数据。
        /// </summary>
        /// <typeparam name="TProjection">投影类的类型</typeparam>
        /// <param name="pageNumber">页码，第几页</param>
        /// <param name="pageSize">页数据大小，每一页多少条数据。</param>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数据分页结果</returns>
        Task<PagedResultWithCount<TProjection>> GetPagedListAndCountAsync<TProjection>(int pageNumber, int pageSize, ISpecification<TAggregateRoot>? specification,
            IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 计算所有聚合根的数量
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数量</returns>
        Task<long> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件，计数相关的聚合根的数量
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根的数量</returns>
        Task<long> CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据查询条件，以及最大限制数量，计数相关的聚合根的数量
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="maxLimit">最大限制数量（0为不限制）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根的计数结果</returns>
        Task<CountResult> CountAsync(ISpecification<TAggregateRoot> specification, long maxLimit, CancellationToken cancellationToken = default);

        /// <summary>
        /// 返回一个<see cref="Boolean"/>值，该值表示符合指定规约条件的聚合根实体对象是否存在。
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果符合指定规约条件的聚合根存在，则返回true，否则返回false。</returns>
        Task<bool> ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default);

    }

    /// <summary>
    /// 只读仓储接口
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IReadOnlyRepository<TAggregateRoot, TKey> : IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 根据聚合根的ID值，从仓储中读取聚合根对象。
        /// 如果对象不存在，抛出异常 <see cref="EntityNotFoundException"/>
        /// </summary>
        /// <param name="key">聚合根的ID值。</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实例。</returns>
        Task<TAggregateRoot?> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据聚合根的ID值，从仓储中读取聚合根对象。 如果对象不存在，返回NULL
        /// </summary>
        /// <param name="key">聚合根的ID值。</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实例。</returns>
        Task<TAggregateRoot?> FindByKeyAsync(TKey key, CancellationToken cancellationToken = default);
    }

}
