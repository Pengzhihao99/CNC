using Framework.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Entities
{
    /// <summary>
    /// 表示实体是需要维护领域事件
    /// </summary>
    public interface IMaintainDomainEvents
    {
        /// <summary>
        /// 获取一个领域事件集合（只读）
        /// </summary>
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <param name="isIgnoreSameType">是否忽略相同类型的领域事件</param>
        void AddDomainEvent(IDomainEvent domainEvent, bool isIgnoreSameType = true);

        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="domainEvent"></param>
        void RemoveDomainEvent(IDomainEvent domainEvent);

        /// <summary>
        /// 清除领域事件
        /// </summary>
        void ClearDomainEvents();
    }
}
