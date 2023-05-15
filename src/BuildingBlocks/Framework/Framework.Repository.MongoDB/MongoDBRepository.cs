using Framework.Domain.Core.Entities;
using Framework.Domain.Core.Events;
using Framework.Domain.Core.Repositories;
using Framework.Domain.Core.Specification;
using Framework.Infrastructure.Crosscutting.IdGenerators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.MongoDB
{
    /// <summary>
    /// Mongodb 仓储
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根实体类型</typeparam>
    public class MongoDBRepository<TAggregateRoot> : ReadOnlyMongoDBRepository<TAggregateRoot>, IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected readonly IDomainEventBus _domainEventBus;

        protected readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// </summary> 
        public MongoDBRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider) : base(mongoDbContext)
        {
            _domainEventBus = domainEventBus;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 将聚合根实体对象添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoot">需要添加到仓储的聚合根实体对象。</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
        {
            //检查Id是否为空，为空尝试赋值
            CheckAndSetId(aggregateRoot);

            if (_mongoDBContext.Session != null)
            {
                await this.GetCollection().InsertOneAsync(_mongoDBContext.Session, aggregateRoot, cancellationToken: cancellationToken);
            }
            else
            {
                await this.GetCollection().InsertOneAsync(aggregateRoot, cancellationToken: cancellationToken);
            }

            //触发领域事件
            await TriggerDomainEventsAsync(aggregateRoot);
        }

        /// <summary>
        /// 批量将指定的聚合根实体对象添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoots">需要添加到仓储的聚合根实例列表。</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task AddAsync(IEnumerable<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken = default)
        {
            var aggregateRootsArray = aggregateRoots?.ToArray() ?? new TAggregateRoot[0];

            if (!aggregateRootsArray.Any())
            {
                return;
            }

            foreach (var entity in aggregateRootsArray)
            {
                CheckAndSetId(entity);
            }

            if (_mongoDBContext.Session != null)
            {
                await this.GetCollection().InsertManyAsync(_mongoDBContext.Session, aggregateRootsArray, new InsertManyOptions(), cancellationToken: cancellationToken);
            }
            else
            {
                await this.GetCollection().InsertManyAsync(aggregateRootsArray, new InsertManyOptions(), cancellationToken);
            }

            //触发领域事件
            foreach (var entity in aggregateRootsArray)
            {
                await TriggerDomainEventsAsync(entity);
            }
        }

        /// <summary>
        /// 更新聚合根实体对象。
        /// </summary>
        /// <param name="aggregateRoot">需要更新的聚合根。</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
        {
            var oldConcurrencyStamp = SetNewConcurrencyStamp(aggregateRoot);

            var filterDefinitions = BuildEntityFilterDefinitionForUpdateOrDelete(aggregateRoot, true, oldConcurrencyStamp);

            if (_mongoDBContext.Session != null)
            {
                await this.GetCollection().ReplaceOneAsync(_mongoDBContext.Session, filterDefinitions, aggregateRoot, cancellationToken: cancellationToken);
            }
            else
            {
                await this.GetCollection().ReplaceOneAsync(filterDefinitions, aggregateRoot, cancellationToken: cancellationToken);
            }

            //触发领域事件
            await TriggerDomainEventsAsync(aggregateRoot);
        }

        /// <summary>
        /// 批量更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoots">需要更新的聚合根实例列表。</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task UpdateAsync(IEnumerable<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken = default)
        {
            var aggregateRootsArray = aggregateRoots?.ToArray() ?? new TAggregateRoot[0];

            if (aggregateRootsArray.Any())
            {
                if (aggregateRootsArray.Length == 1)
                {
                    await UpdateAsync(aggregateRootsArray[0], cancellationToken);
                    return;
                }

                var writeModels = new List<WriteModel<TAggregateRoot>>();

                foreach (var aggregateRoot in aggregateRootsArray)
                {
                    var oldConcurrencyStamp = SetNewConcurrencyStamp(aggregateRoot);

                    var filterDefinitions = BuildEntityFilterDefinitionForUpdateOrDelete(aggregateRoot, true, oldConcurrencyStamp);

                    writeModels.Add(new ReplaceOneModel<TAggregateRoot>(filterDefinitions, aggregateRoot));
                }

                //批量修改
                BulkWriteResult<TAggregateRoot> bulkWriteResult;

                if (_mongoDBContext.Session != null)
                {
                    bulkWriteResult = await this.GetCollection().BulkWriteAsync(_mongoDBContext.Session, writeModels, cancellationToken: cancellationToken);
                }
                else
                {
                    bulkWriteResult = await this.GetCollection().BulkWriteAsync(writeModels, cancellationToken: cancellationToken);
                }

                // bulkWriteResult.RequestCount 请求的数量 writeModels的数量
                // bulkWriteResult.MatchedCount 匹配的数量 writeModels中Filter匹配到的数量
                // bulkWriteResult.ModifiedCount 处理的数量
                // bulkWriteResult.ProcessedRequests 已处理的请求

                //如果批量修改，部分成功，则认为失败，存在并发性错误（ID不匹配，或者数据被删除），出现这种问题的情况下，一般是 没有基于 Mongodb 事务的情况下
                if (bulkWriteResult.ProcessedRequests.Count != aggregateRootsArray.Length || bulkWriteResult.MatchedCount != aggregateRootsArray.Length)
                {
                    //throw new ConcurrencyException($"批量更新类型 [{objs.First().GetType()}] 发生并发性错误");
                    throw new Exception($"批量更新类型 [{aggregateRootsArray.First().GetType()}] 出现数量不匹配，请求的数量：[{bulkWriteResult.RequestCount}], 已处理的请求：[{bulkWriteResult.ProcessedRequests.Count}], " +
                                        $"匹配的数量：[{bulkWriteResult.MatchedCount}], 修改的数量：[{bulkWriteResult.ModifiedCount}]。");
                }

                //触发领域事件
                foreach (var entity in aggregateRootsArray)
                {
                    await TriggerDomainEventsAsync(entity);
                }
            }
        }

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoot">需要从仓储中移除的聚合根。</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
        {
            var oldConcurrencyStamp = SetNewConcurrencyStamp(aggregateRoot);

            var filterDefinitions = BuildEntityFilterDefinitionForUpdateOrDelete(aggregateRoot, true, oldConcurrencyStamp);

            if (_mongoDBContext.Session != null)
            {
                await this.GetCollection().DeleteOneAsync(_mongoDBContext.Session, filterDefinitions, cancellationToken: cancellationToken);
            }
            else
            {
                await this.GetCollection().DeleteOneAsync(filterDefinitions, cancellationToken: cancellationToken);
            }

            //触发领域事件
            await TriggerDomainEventsAsync(aggregateRoot);
        }

        /// <summary>
        /// 批量将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoots">需要从仓储中移除的聚合根实例列表。</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task RemoveAsync(IList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken = default)
        {
            var aggregateRootsArray = aggregateRoots?.ToArray() ?? new TAggregateRoot[0];

            if (aggregateRootsArray.Any())
            {
                if (aggregateRootsArray.Length == 1)
                {
                    await RemoveAsync(aggregateRootsArray[0], cancellationToken);
                    return;
                }

                var aggreagteRootConcurrencyStamptTuples = new List<Tuple<TAggregateRoot, string>>();

                foreach (var aggregateRoot in aggregateRootsArray)
                {
                    var oldConcurrencyStamp = SetNewConcurrencyStamp(aggregateRoot);
                    aggreagteRootConcurrencyStamptTuples.Add(new Tuple<TAggregateRoot, string>(aggregateRoot, oldConcurrencyStamp));
                }

                //_mongoDBContext.Session == null的时候，withConcurrencyStamp才为true
                var filterDefinitions = BuildEntitiesFilterDefinitionForDelete(aggreagteRootConcurrencyStamptTuples, _mongoDBContext.Session == null);

                if (_mongoDBContext.Session != null)
                {
                    await this.GetCollection().DeleteManyAsync(_mongoDBContext.Session, filterDefinitions, cancellationToken: cancellationToken);
                }
                else
                {
                    await this.GetCollection().DeleteManyAsync(filterDefinitions, cancellationToken: cancellationToken);
                }

                //触发领域事件
                foreach (var entity in aggregateRootsArray)
                {
                    await TriggerDomainEventsAsync(entity);
                }
            }
        }

        /// <summary>
        /// 批量将符合条件的聚合对象从仓储中移除。
        /// </summary>
        /// <param name="specification">条件参数。</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task RemoveAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
        {
            if (_mongoDBContext.Session != null)
            {
                await this.GetCollection().DeleteManyAsync(_mongoDBContext.Session, specification.GetExpression(), cancellationToken: cancellationToken);
            }
            else
            {
                await this.GetCollection().DeleteManyAsync(specification.GetExpression(), cancellationToken: cancellationToken);
            }
        }

        /// <summary>
        /// 构建用于实体修改或者删除时候的过滤条件
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="withConcurrencyStamp"></param>
        /// <param name="concurrencyStamp"></param>
        /// <returns></returns>
        protected virtual FilterDefinition<TAggregateRoot> BuildEntityFilterDefinitionForUpdateOrDelete(TAggregateRoot entity, bool withConcurrencyStamp = false, string concurrencyStamp = null)
        {
            var builder = Builders<TAggregateRoot>.Filter;

            var filterDefinitions = new List<FilterDefinition<TAggregateRoot>>
            {
                //根据过滤
                BuildIdentificationFilterDefinition(entity)
            };

            //乐观并发
            if (withConcurrencyStamp && entity is IHasConcurrencyStamp entityWithConcurrencyStamp)
            {
                if (concurrencyStamp == null)
                {
                    concurrencyStamp = entityWithConcurrencyStamp.ConcurrencyStamp;
                }

                filterDefinitions.Add(builder.Eq(e => ((IHasConcurrencyStamp)e).ConcurrencyStamp, concurrencyStamp));
            }

            //分片过滤
            //var shardKeysFilterDefinitions = BuildShardKeysFilterDefinitions(entity, true);

            //if (shardKeysFilterDefinitions.Any())
            //{
            //    filterDefinitions.AddRange(shardKeysFilterDefinitions);
            //}

            return filterDefinitions.Count == 1 ? filterDefinitions.First() : Builders<TAggregateRoot>.Filter.And(filterDefinitions);
        }

        /// <summary>
        /// 构建实体的唯一标识的过滤条件，用于修改或者删除时候
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        protected virtual FilterDefinition<TAggregateRoot> BuildIdentificationFilterDefinition(TAggregateRoot entity)
        {
            throw new NotImplementedException(
                $"{nameof(BuildIdentificationFilterDefinition)} is not implemented for MongoDB by default. It should be overriden and implemented by the deriving class!"
            );
        }

        /// <summary>
        /// 构建实体对象分片键过滤条件，用于分片模式下删除和修改对象时候的条件。
        /// 参阅官方文档:https://docs.mongodb.com/manual/reference/method/db.collection.update/
        /// 在分片部署的情况下，使用findAndModify的方法去修改一个分片集合下的Document的时候，如果过滤条件没有带上分片键，会报一个错误：
        /// <para>
        ///  MongoDB.Driver.MongoCommandException : Command findAndModify failed: Query for sharded findAndModify must contain the shard key.
        /// </para>+
        /// 在Mongodb 4.2 版本开始，使用update或者replace，一个分片集合的Document的时候，需要包含完整的shard key作为过滤条件，用于定位分区片块。
        /// 不过，在Mongodb 4.4 版本开始，可以忽略包含完整的shard key作为过滤条件, 但是如果是 findAndModify ，还是不可以。
        /// </summary>
        /// <param name="entity">聚合根实体对象</param>
        /// <param name="ignoreId">是否忽略_id字段，一般在外层可能已经增加了这个判断条件</param>
        /// <returns>过滤条件列表</returns>
        //protected virtual IList<FilterDefinition<TAggregateRoot>> BuildShardKeysFilterDefinitions(TAggregateRoot entity, bool ignoreId)
        //{
        //    //判断是否是分片集合
        //    var entityTypeConfiguration = _mongoDBContext.GetEntityTypeConfiguration<TAggregateRoot>();

        //    var filterDefinitions = new List<FilterDefinition<TAggregateRoot>>();

        //    if (entityTypeConfiguration.IsShardCollection && entityTypeConfiguration.ShardKeys.Any())
        //    {
        //        foreach (var bsonElement in entityTypeConfiguration.ShardKeys)
        //        {
        //            //过滤掉ID，因为Id条件在上方已经加入到list
        //            if (bsonElement.Name != "_id")
        //            {
        //                filterDefinitions.Add(new BsonDocument(bsonElement.Name, GetValueByShardKey(entity, bsonElement.Name)));
        //            }
        //        }
        //    }

        //    return filterDefinitions;
        //}

        /// <summary>
        /// 构建用于实体批量删除时候的过滤条件
        /// </summary>
        /// <param name="entities">实体对象聚合</param>
        /// <param name="withConcurrencyStamp">是否有并发戳</param>
        protected virtual FilterDefinition<TAggregateRoot> BuildEntitiesFilterDefinitionForDelete(IEnumerable<Tuple<TAggregateRoot, string>> entities, bool withConcurrencyStamp = false)
        {
            throw new NotImplementedException(
                $"{nameof(BuildEntitiesFilterDefinitionForDelete)} is not implemented for MongoDB by default. It should be overriden and implemented by the deriving class!"
            );
        }

        /// <summary>
        /// 如果实体是实现了  <see cref="IHasConcurrencyStamp"/> 接口的，则为其接口下定义的一个属性（ <see cref="IHasConcurrencyStamp.ConcurrencyStamp"/>）赋值一个新的值。用于并发校验。
        /// </summary>
        /// <param name="entity">聚合根实体</param>
        /// <returns>属性（ <see cref="IHasConcurrencyStamp.ConcurrencyStamp"/>）的旧值</returns>
        protected virtual string? SetNewConcurrencyStamp(IAggregateRoot entity)
        {
            if (!(entity is IHasConcurrencyStamp concurrencyStampEntity))
            {
                return null;
            }

            var oldConcurrencyStamp = concurrencyStampEntity.ConcurrencyStamp;
            concurrencyStampEntity.ConcurrencyStamp = Guid.NewGuid().ToString("N"); //赋值一个GUID字符串值

            return oldConcurrencyStamp;
        }

        /// <summary>
        /// 检查和设置Id
        /// </summary>
        /// <param name="entity">聚合根实体</param>
        protected virtual void CheckAndSetId(TAggregateRoot entity)
        {
            //子类实现
        }

        /// <summary>
        /// 触发领域事件
        /// </summary>
        protected virtual async Task TriggerDomainEventsAsync(IAggregateRoot entity)
        {
            var maintainDomainEventsEntity = entity as IMaintainDomainEvents;

            if (maintainDomainEventsEntity == null)
            {
                return;
            }

            await _domainEventBus.PublishAsync(maintainDomainEventsEntity.DomainEvents);
            //发布完了 领域事件，清空聚合根中的领域事件
            maintainDomainEventsEntity.ClearDomainEvents();
        }

        /// <summary>
        /// 根据分片key的字段，获取对应的聚合根对象中对应的值
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
        /// <param name="obj">聚合根对象</param>
        /// <param name="shardKey">分片键值</param>
        private BsonValue GetValueByShardKey(TAggregateRoot obj, string shardKey)
        {
            //聚合根的BsonClassMap
            var aggregateRootTypeBsonClassMap = BsonClassMap.LookupClassMap(typeof(TAggregateRoot));

            //普通字段
            if (!shardKey.Contains("."))
            {
                var value = aggregateRootTypeBsonClassMap.GetMemberMapForElement(shardKey).Getter(obj);
                return BsonValue.Create(value);
            }
            else
            {
                //如果字段是在子文档里面，根据点号分割
                var spKeys = shardKey.Split(separator: '.');

                var preMap = aggregateRootTypeBsonClassMap;
                object preObj = obj;

                for (int i = 0; i < spKeys.Length; i++)
                {
                    //获取成员Map
                    var memberMap = preMap.GetMemberMapForElement(spKeys[i]);

                    if (i == (spKeys.Length - 1))
                    {
                        var value = memberMap.Getter(preObj);
                        return BsonValue.Create(value);
                    }
                    else
                    {
                        preObj = memberMap.Getter(preObj);
                        preMap = BsonClassMap.LookupClassMap(memberMap.MemberType);
                    }
                }
            }

            return BsonValue.Create(null);
        }

    }

    /// <summary>
    /// Mongodb 仓储
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根实体类型</typeparam>
    public class MongoDBRepository<TAggregateRoot, TKey> : MongoDBRepository<TAggregateRoot>,
        IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// </summary> 
        public MongoDBRepository(IMongoDBContext mongoDbContext, IDomainEventBus domainEventBus, IServiceProvider serviceProvider)
            : base(mongoDbContext, domainEventBus, serviceProvider)
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

        /// <summary>
        /// 检查和设置Id
        /// </summary>
        /// <param name="entity">聚合根实体</param>
        protected override void CheckAndSetId(TAggregateRoot entity)
        {
            //如果ID非空且有值
            if (entity.Id != null && !entity.Id.Equals(default(TKey)))
            {
                return;
            }

            //无值，则尝试设置Id值
            var identityGenerator = (IIdentityGenerator<TKey>)_serviceProvider.GetService(typeof(IIdentityGenerator<TKey>));

            if (identityGenerator != null)
            {
                //尝试设置Id
                entity.Id = identityGenerator.GenerateId();
                return;
            }

            throw new Exception($"未给{entity.GetType().Name}对象赋值Id值");
        }

        /// <summary>
        /// 构建实体的唯一标识的过滤条件，用于修改或者删除时候
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        protected override FilterDefinition<TAggregateRoot> BuildIdentificationFilterDefinition(TAggregateRoot entity)
        {
            //根据ID过滤
            return Builders<TAggregateRoot>.Filter.Eq(c => c.Id, entity.Id);
        }

        /// <summary>
        /// 构建用于实体批量删除时候的过滤条件
        /// </summary>
        /// <param name="entities">实体对象聚合</param>
        /// <param name="withConcurrencyStamp">是否有并发戳</param>
        /// <returns></returns>
        protected override FilterDefinition<TAggregateRoot> BuildEntitiesFilterDefinitionForDelete(IEnumerable<Tuple<TAggregateRoot, string>> entities, bool withConcurrencyStamp = false)
        {
            var builder = Builders<TAggregateRoot>.Filter;

            if (!withConcurrencyStamp)
            {
                var deletedIds = entities.Select(x => x.Item1.Id).Distinct().ToList();
                var filter = builder.In(c => c.Id, deletedIds);

                return filter;
            }

            //TODO：这种方式可能在数据来过大的情况下，存在问题。
            var filterDefinitions = new List<FilterDefinition<TAggregateRoot>>();

            foreach (var entity in entities)
            {
                filterDefinitions.Add(builder.And(
                    builder.Eq(c => c.Id, entity.Item1.Id),
                    builder.Eq(e => ((IHasConcurrencyStamp)e).ConcurrencyStamp, entity.Item2)));
            }

            return builder.Or(filterDefinitions);
        }
    }
}
