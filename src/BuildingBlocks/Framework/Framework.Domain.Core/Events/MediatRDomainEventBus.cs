using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Core.Events
{
    /// <summary>
    /// 基于 MediatR 来实现的领域事件总线
    /// </summary>
    public class MediatRDomainEventBus : IDomainEventBus
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// </summary> 
        public MediatRDomainEventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 发布领域事件
        /// </summary>
        /// <param name="domainEvent">领域事件对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task PublishAsync(IDomainEvent domainEvent,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return _mediator.Publish(domainEvent, cancellationToken);
        }

        /// <summary>
        /// 发布领域事件
        /// </summary>
        /// <typeparam name="TDomainEvent">领域事件</typeparam>
        /// <param name="event">领域事件对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        public Task PublishAsync<TDomainEvent>(TDomainEvent @event, CancellationToken cancellationToken = default(CancellationToken)) where TDomainEvent : IDomainEvent
        {
            return _mediator.Publish(@event, cancellationToken);
        }

        /// <summary>
        /// 批量发布多个领域事件
        /// </summary>
        /// <param name="domainEvents">领域事件对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task PublishAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default(CancellationToken))
        {
            //将 领域事件 顺序执行 
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }

        /// <summary>
        /// 批量发布多个领域事件
        /// </summary>
        /// <typeparam name="TDomainEvent">领域事件</typeparam>
        /// <param name="domainEvents">领域事件对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task PublishAsync<TDomainEvent>(IEnumerable<TDomainEvent> domainEvents, CancellationToken cancellationToken = default(CancellationToken)) where TDomainEvent : IDomainEvent
        {
            //将 领域事件 顺序执行 
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            ////多线程执行领域事件
            //var tasks = domainEvents
            //    .Select(async (domainEvent) =>
            //    {
            //        await _mediator.Publish(domainEvent, cancellationToken);
            //    });

            //await Task.WhenAll(tasks);
        }
    }
}
