using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Infrastructure.Crosscutting.EventBus
{
    /// <summary>
    /// 表示一个集成事件
    /// </summary>
    public abstract class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }


        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        public Guid Id { get; private set; }

        public DateTime CreationDate { get; private set; }
    }
}
