using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Infrastructure.Crosscutting.EventBus.RabbitMq.Queues
{
    [Serializable]
    public class DeadLetterQueue
    {
        public string RoutingKey { get; set; }
        public string Exchange { get; set; }

        public string Queue { get; set; }

        public string Exception { get; set; }

        public DateTime CreateOn { get; set; }

        public string Message { get; set; }

        public string Id { get; set; }
    }
}
