using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Infrastructure.Crosscutting.EventBus.RabbitMQ
{
    public class RabbitMQEventBusSettings
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string ExchangeName { get; set; }

        public int ConnectRetryCount { get; set; }

        public ushort PrefetchCount { get; set; }
    }
}
