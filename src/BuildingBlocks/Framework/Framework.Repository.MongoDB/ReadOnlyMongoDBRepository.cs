using Framework.Domain.Core.Entities;
using Framework.Domain.Core.Repositories;
using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB
{
    /// <summary>
    /// Mongodb 只读仓储
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根实体类型</typeparam>
    public class ReadOnlyMongoDBRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected readonly IMongoDBContext _mongoDBContext;

        protected IMongoCollection<TAggregateRoot> _mongoCollection;

        /// <summary>
        /// MongoDB仓储上下文
        /// </summary> 
        public ReadOnlyMongoDBRepository(IMongoDBContext mongoDbContext)
        {
            _mongoDBContext = mongoDbContext;
        }

        protected IMongoCollection<TAggregateRoot> GetCollection()
        {
            _mongoCollection ??= _mongoDBContext.GetCollection<TAggregateRoot>();
            return _mongoCollection;
        }

        /// <summary>
        /// 通过条件获取一个聚合根实体对象，若匹配多个，则返回第一个。
        /// 如果对象不存在，抛出异常 <see cref="EntityNotFoundException"/>
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实例</returns>
        public virtual async Task<TAggregateRoot?> GetAsync(ISpecification<TAggregateRoot>? specification, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(specification, cancellationToken);

            if (entity == null)
            {
                //throw new EntityNotFoundException();
                return null;
            }

            return entity;
        }


        /// <summary>
        /// 通过条件获取一个聚合根实体对象，若匹配多个，则返回第一个。 如果对象不存在，返回NULL
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实例</returns>
        public virtual async Task<TAggregateRoot> FindAsync(ISpecification<TAggregateRoot>? specification, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();

            specification ??= Specification<TAggregateRoot>.Eval(x => true);

            if (_mongoDBContext.Session != null)
            {
                return await collection.Find(_mongoDBContext.Session, specification.GetExpression()).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            }

            return await collection.Find(specification.GetExpression()).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 获取所有聚合根实体对象的列表
        /// </summary>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实体列表</returns>
        public virtual Task<IList<TAggregateRoot>> GetListAsync(IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default)
        {
            return GetListAsync<TAggregateRoot>(sortCriterias, cancellationToken);
        }

        /// <summary>
        /// 获取所有聚合根实体对象, 返回的其【投影】对象的列表
        /// </summary>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实体投影实例的列表</returns>
        public virtual async Task<IList<TProjection>> GetListAsync<TProjection>(IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default) where TProjection : class
        {
            var collection = GetCollection();

            var findOption = new FindOptions<TAggregateRoot, TProjection>();

            //投影
            if (typeof(TProjection) != typeof(TAggregateRoot))
            {
                findOption.Projection = BuildProjectionDefinition<TProjection>();
            }

            //排序
            if (sortCriterias != null)
            {
                findOption.Sort = BuildSortDefinition(sortCriterias);
            }

            IAsyncCursor<TProjection> asyncCursor;

            if (_mongoDBContext.Session != null)
            {
                asyncCursor = await collection.FindAsync(_mongoDBContext.Session, Builders<TAggregateRoot>.Filter.Empty, findOption, cancellationToken);
            }
            else
            {
                asyncCursor = await collection.FindAsync(Builders<TAggregateRoot>.Filter.Empty, findOption, cancellationToken);
            }

            return asyncCursor.ToList();
        }

        /// <summary>
        /// 根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体数据.
        /// 若指定了最大返回数量（参数 <paramref name="maxResultCount"/>）, 则最大返回指定数量的结果集。 如目标数据的数量有100个，但是指定只返回前20个。
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="maxResultCount">获取的最大数量限制，默认为0，表示不限制</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实体列表</returns>
        public virtual Task<IList<TAggregateRoot>> GetListAsync(ISpecification<TAggregateRoot>? specification, IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, int maxResultCount = 0,
            CancellationToken cancellationToken = default)
        {
            return GetListAsync<TAggregateRoot>(specification, sortCriterias, maxResultCount, cancellationToken);
        }

        /// <summary>
        /// 根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体, 返回的其【投影】对象的列表
        /// 若指定了最大返回数量（参数 <paramref name="maxResultCount"/>）, 则最大返回指定数量的结果集。 如目标数据的数量有100个，但是指定只返回前20个。
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="maxResultCount">获取的最大数量限制，默认为0，表示不限制</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实体列表</returns>
        public virtual async Task<IList<TProjection>> GetListAsync<TProjection>(ISpecification<TAggregateRoot>? specification, IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, int maxResultCount = 0,
            CancellationToken cancellationToken = default) where TProjection : class
        {
            var collection = GetCollection();

            if (specification == null)
            {
                specification = Specification<TAggregateRoot>.Eval(x => true);
            }

            var findOption = new FindOptions<TAggregateRoot, TProjection>();

            //投影
            if (typeof(TProjection) != typeof(TAggregateRoot))
            {
                findOption.Projection = BuildProjectionDefinition<TProjection>();
            }

            //排序
            if (sortCriterias != null)
            {
                findOption.Sort = BuildSortDefinition(sortCriterias);
            }

            if (maxResultCount > 0)
            {
                findOption.Limit = maxResultCount;
            }

            IAsyncCursor<TProjection> asyncCursor;

            if (_mongoDBContext.Session != null)
            {
                asyncCursor = await collection.FindAsync(_mongoDBContext.Session, specification.GetExpression(), findOption, cancellationToken);
            }
            else
            {
                asyncCursor = await collection.FindAsync(specification.GetExpression(), findOption, cancellationToken);
            }

            return asyncCursor.ToList();
        }

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
        public virtual Task<PagedResult<TAggregateRoot>> GetPagedListAsync(int pageNumber, int pageSize, ISpecification<TAggregateRoot>? specification, IList<SortCriteria<TAggregateRoot>>? sortCriterias = null,
            CancellationToken cancellationToken = default)
        {
            return GetPagedListAsync<TAggregateRoot>(pageNumber, pageSize, specification, sortCriterias, cancellationToken);
        }

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
        public virtual async Task<PagedResult<TProjection>> GetPagedListAsync<TProjection>(int pageNumber, int pageSize, ISpecification<TAggregateRoot>? specification,
            IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default) where TProjection : class
        {
            ValidPageNumberAndSize(pageNumber, pageSize);

            //查询总数
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;

            specification ??= Specification<TAggregateRoot>.Eval(x => true);

            //排序构建
            var sortDefinition = BuildSortDefinition(sortCriterias);
            var pageList = await FindAndGetList<TProjection>(skip, take, specification, sortDefinition);

            return new PagedResult<TProjection>(pageNumber, pageSize, pageList);
        }

        /// <summary>
        /// 分页查询数据，根据指定的规约，排序字段和排序方式，查询符合条件的聚合根实体数据，同时计算数据的总数。根据指定的第几页（<paramref name="pageNumber"/>）和每一页数量（<paramref name="pageSize"/>），返回匹配区间的聚合根实体对象列表数据。
        /// </summary>
        /// <param name="pageNumber">页码，第几页</param>
        /// <param name="pageSize">页数据大小，每一页多少条数据。</param>
        /// <param name="specification">查询条件规约</param>
        /// <param name="sortCriterias">排序条件（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数据分页结果</returns>
        public virtual Task<PagedResultWithCount<TAggregateRoot>> GetPagedListAndCountAsync(int pageNumber, int pageSize, ISpecification<TAggregateRoot>? specification, IList<SortCriteria<TAggregateRoot>>? sortCriterias = null,
            CancellationToken cancellationToken = default)
        {
            return GetPagedListAndCountAsync<TAggregateRoot>(pageNumber, pageSize, specification, sortCriterias, cancellationToken);
        }

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
        public virtual async Task<PagedResultWithCount<TProjection>> GetPagedListAndCountAsync<TProjection>(int pageNumber, int pageSize, ISpecification<TAggregateRoot>? specification,
            IList<SortCriteria<TAggregateRoot>>? sortCriterias = null, CancellationToken cancellationToken = default)
        {
            ValidPageNumberAndSize(pageNumber, pageSize);

            //查询总数
            var totalCount = await CountAsync(specification);

            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            int totalPages = ((int)totalCount + pageSize - 1) / pageSize;

            specification ??= Specification<TAggregateRoot>.Eval(x => true);

            //排序构建
            var sortDefinition = BuildSortDefinition(sortCriterias);
            var pageList = await FindAndGetList<TProjection>(skip, take, specification, sortDefinition);

            return new PagedResultWithCount<TProjection>(totalCount, totalPages, pageNumber, pageSize, pageList);
        }

        /// <summary>
        /// 计算所有聚合根的数量
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>数量</returns>
        public virtual async Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            return await collection.EstimatedDocumentCountAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 根据条件，计数相关的聚合根的数量
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根的数量</returns>
        public virtual async Task<long> CountAsync(ISpecification<TAggregateRoot>? specification, CancellationToken cancellationToken = default)
        {
            if (specification == null)
            {
                return await CountAsync(cancellationToken);
            }

            var collection = GetCollection();

            if (_mongoDBContext.Session != null)
            {
                return await collection.CountDocumentsAsync(_mongoDBContext.Session, specification.GetExpression(), cancellationToken: cancellationToken);
            }

            return await collection.CountDocumentsAsync(specification.GetExpression(), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 根据查询条件，以及最大限制数量，计数相关的聚合根的数量
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="maxLimit">最大限制数量（0为不限制）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根的计数结果</returns>
        public virtual async Task<CountResult> CountAsync(ISpecification<TAggregateRoot>? specification, long maxLimit, CancellationToken cancellationToken = default)
        {
            long count = 0;

            if (maxLimit <= 0)
            {
                count = await this.CountAsync(specification, cancellationToken);
                return new CountResult(count, 0, false);
            }

            var countOptions = new CountOptions()
            {
                Limit = maxLimit + 1
            };

            var collection = this.GetCollection();

            specification ??= Specification<TAggregateRoot>.Eval(x => true);

            if (_mongoDBContext.Session != null)
            {
                count = await collection.CountDocumentsAsync(_mongoDBContext.Session, specification.GetExpression(), countOptions, cancellationToken);
            }
            else
            {
                count = await collection.CountDocumentsAsync(specification.GetExpression(), countOptions, cancellationToken);
            }

            return new CountResult(count > maxLimit ? maxLimit : count, maxLimit, count > maxLimit);
        }

        /// <summary>
        /// 返回一个<see cref="Boolean"/>值，该值表示符合指定规约条件的聚合根实体对象是否存在。
        /// </summary>
        /// <param name="specification">查询条件规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果符合指定规约条件的聚合根存在，则返回true，否则返回false。</returns>
        public virtual async Task<bool> ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
        {
            Check.Argument.IsNotNull(specification, "specification");

            var collection = GetCollection();

            if (_mongoDBContext.Session != null)
            {
                return (await collection.Find(_mongoDBContext.Session, specification.GetExpression()).FirstOrDefaultAsync(cancellationToken: cancellationToken)) != null;
            }

            return (await collection.Find(specification.GetExpression()).FirstOrDefaultAsync(cancellationToken: cancellationToken)) != null;
        }

        protected virtual void ValidPageNumberAndSize(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");
            }
        }

        /// <summary>
        /// 构建排序定义 SortDefinition
        /// </summary>
        /// <param name="sortCriterias">排序条件</param>
        /// <returns></returns>
        protected virtual SortDefinition<TAggregateRoot>? BuildSortDefinition(IList<SortCriteria<TAggregateRoot>>? sortCriterias)
        {
            //排序构建
            var sortBuilder = Builders<TAggregateRoot>.Sort;
            SortDefinition<TAggregateRoot>? sortDefinition = null;

            if (sortCriterias != null && sortCriterias.Count > 0)
            {
                foreach (var item in sortCriterias)
                {
                    if (sortDefinition == null)
                    {
                        sortDefinition = item.SortOrder == SortOrder.Descending ? sortBuilder.Descending(item.SortKeySelector) : sortBuilder.Ascending(item.SortKeySelector);
                    }
                    else
                    {
                        sortDefinition = item.SortOrder == SortOrder.Descending ? sortDefinition.Descending(item.SortKeySelector) : sortDefinition.Ascending(item.SortKeySelector);
                    }
                }
            }

            return sortDefinition;
        }

        /// <summary>
        /// 创建投影 ProjectionDefinition
        /// </summary>
        /// <typeparam name="TProjection">投影类型</typeparam>
        protected virtual ProjectionDefinition<TAggregateRoot, TProjection> BuildProjectionDefinition<TProjection>()
        {
            //查询投影对象的属性
            var properties = typeof(TProjection).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var fieldList = new List<ProjectionDefinition<TAggregateRoot>>();

            //遍历
            foreach (var proeProperty in properties)
            {
                fieldList.Add(Builders<TAggregateRoot>.Projection.Include(proeProperty.Name.ToString()));
            }

            var projection = Builders<TAggregateRoot>.Projection.Combine(fieldList);

            //如果没有主键，需要特地排除
            if (properties.All(x => x.Name != "Id"))
            {
                //projection = projection.Exclude(x => x.Id);
                projection = projection.Exclude("Id");
            }

            return projection;
        }

        private async Task<List<TProjection>> FindAndGetList<TProjection>(int skip, int take, ISpecification<TAggregateRoot> specification, SortDefinition<TAggregateRoot>? sortDefinition)
        {
            IAsyncCursor<TProjection> asyncCursor;
            var collection = this.GetCollection();

            var findOptions = new FindOptions<TAggregateRoot, TProjection>()
            {
                Limit = take,
                Skip = skip,
                Sort = sortDefinition,
            };

            if (typeof(TProjection) != typeof(TAggregateRoot))
            {
                findOptions.Projection = BuildProjectionDefinition<TProjection>();
            }

            if (_mongoDBContext.Session != null)
            {
                asyncCursor = await collection.FindAsync(_mongoDBContext.Session, specification.GetExpression(), findOptions);
            }
            else
            {
                asyncCursor = await collection.FindAsync(specification.GetExpression(), findOptions);
            }

            return asyncCursor.ToList();
        }
    }

    /// <summary>
    ///  Mongodb 只读仓储
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根实体类型</typeparam>
    /// <typeparam name="TKey">聚合根主键</typeparam>
    public class ReadOnlyMongoDBRepository<TAggregateRoot, TKey> : ReadOnlyMongoDBRepository<TAggregateRoot>, IReadOnlyRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        public ReadOnlyMongoDBRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
        {
        }

        /// <summary>
        /// 根据聚合根的ID值，从仓储中读取聚合根对象。
        /// 如果对象不存在，抛出异常 <see cref="EntityNotFoundException"/>
        /// </summary>
        /// <param name="key">聚合根的ID值。</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实例。</returns>
        public virtual async Task<TAggregateRoot?> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var entity = await FindByKeyAsync(key, cancellationToken);

            if (entity == null)
            {
                //throw new EntityNotFoundException();
                return null;
            }

            return entity;
        }

        /// <summary>
        /// 根据聚合根的ID值，从仓储中读取聚合根对象。 如果对象不存在，返回NULL
        /// </summary>
        /// <param name="key">聚合根的ID值。</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>聚合根实例。</returns>
        public virtual async Task<TAggregateRoot?> FindByKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var collection = this.GetCollection();

            var filter = Builders<TAggregateRoot>.Filter.Eq(c => c.Id, key);

            if (_mongoDBContext.Session != null)
            {
                return await collection.Find(_mongoDBContext.Session, filter).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            }

            return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

    }
}
