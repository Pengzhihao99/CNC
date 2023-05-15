using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Framework.Domain.Core.Events
{
    public interface IDomainEvent : INotification
    {
        /// <summary>
        /// 事件发生时间
        /// </summary>
        DateTime EventTime { get; }
    }
}
