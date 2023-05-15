using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Events
{
    /// <summary>
    /// 领域事件的事件总线
    /// </summary>
    public interface IDomainEventBus
    {
        /// <summary>
        /// 发布领域事件
        /// </summary>
        /// <param name="domainEvent">领域事件对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 发布领域事件
        /// </summary>
        /// <param name="domainEvents">领域事件对象列表</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task PublishAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 发布领域事件
        /// </summary>
        /// <typeparam name="TDomainEvent">领域事件对象</typeparam>
        /// <param name="domainEvent">领域事件对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default(CancellationToken)) where TDomainEvent : IDomainEvent;

        /// <summary>
        /// 批量发布多个领域事件
        /// </summary>
        /// <typeparam name="TDomainEvent">领域事件</typeparam>
        /// <param name="domainEvents">领域事件对象列表</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task PublishAsync<TDomainEvent>(IEnumerable<TDomainEvent> domainEvents, CancellationToken cancellationToken = default(CancellationToken)) where TDomainEvent : IDomainEvent;
    }
}
