using Framework.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Entities
{
    /// <summary>
    /// 聚合根基类，具有一个Id属性作为其唯一标识。
    /// </summary>
    [Serializable]
    public abstract class AggregateRootBase<TKey> : EntityBase<TKey>, IAggregateRoot<TKey>, IMaintainDomainEvents, IHasConcurrencyStamp
        where TKey : IEquatable<TKey>
    {
        protected AggregateRootBase()
        {
            //生成一个GUID 作为并发戳
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            _domainEvents = new List<IDomainEvent>();
        }

        /// <summary>
        /// 并发戳
        /// 避免并发问题所必需的，类似于RowVersion
        /// </summary>
        public string ConcurrencyStamp { get; set; }

        #region domainEvent

        /// <summary>
        /// 领域事件列表
        /// </summary>
        private List<IDomainEvent> _domainEvents;

        /// <summary>
        /// 领域事件集合
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get
            {
                if (_domainEvents == null)
                {
                    _domainEvents = new List<IDomainEvent>();
                }
                return _domainEvents.AsReadOnly();
            }
        }


        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <param name="isIgnoreSameType">是否忽略相同类型的</param>
        public virtual void AddDomainEvent(IDomainEvent domainEvent, bool isIgnoreSameType = true)
        {
            if (isIgnoreSameType && _domainEvents.Any(x => x.GetType() == domainEvent.GetType()))
            {
                return;
            }

            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="domainEvent"></param>
        public virtual void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        /// <summary>
        /// 清除领域事件
        /// </summary>
        public virtual void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        #endregion
    }
}
