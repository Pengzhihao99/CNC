
using Framework.Infrastructure.Crosscutting.EventBus.RabbitMq.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Crosscutting.EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private IModel _consumerChannel;

        private readonly IEventBusSubscriptionManager _subsManager;

        
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventBusRabbitMQ> _logger;

        private readonly RabbitMQEventBusSettings _rabbitMQEventBusSettings;
        private readonly string _extrangeName;
        private readonly int _retryCount;


        public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, ILogger<EventBusRabbitMQ> logger,
            IEventBusSubscriptionManager subsManager, IServiceProvider serviceProvider,
            IOptions<RabbitMQEventBusSettings> rabbitMQEventBusSettings)
        {
            _rabbitMQEventBusSettings = rabbitMQEventBusSettings.Value;
            _retryCount = _rabbitMQEventBusSettings.ConnectRetryCount < 1 ? 5 : _rabbitMQEventBusSettings.ConnectRetryCount;
            _extrangeName = _rabbitMQEventBusSettings.ExchangeName;

            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? throw new ArgumentNullException(nameof(logger));

            _consumerChannel = CreateConsumerChannel();
            _serviceProvider = serviceProvider;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
            
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: eventName,
                    exchange: _extrangeName,
                    routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    eventName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name;

            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using (var channel = _persistentConnection.CreateModel())
            {
                _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

                channel.ExchangeDeclare(exchange: _extrangeName, type: "direct", durable: true);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent
                    properties.ContentType = "application/json";
                    //properties.Type = "";

                    _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

                    channel.BasicPublish(
                        exchange: _extrangeName,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        /// <summary>
        /// 内部使用的死信队列
        /// </summary>
        /// <param name="event"></param>
        private void PublishToDeadLetter(DeadLetterQueue @event)
        {
            try
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "Could not publish DeadLetter event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                    });

                var eventName = @event.GetType().Name;

                _logger.LogTrace("Creating RabbitMQ channel to publish DeadLetter event: {EventId} ({EventName})", @event.Id, eventName);

                using (var channel = _persistentConnection.CreateModel())
                {
                    _logger.LogTrace("Declaring RabbitMQ exchange to publish DeadLetter event: {EventId}", @event.Id);

                    var deadLetterExtrange = _extrangeName + "_DeadLetterExtrange";

                    #region 死信
                    channel.ExchangeDeclare(exchange: deadLetterExtrange, type: "fanout", durable: true);

                    channel.QueueDeclare(queue: eventName,
                                          durable: true,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null
                                          );

                    channel.QueueBind(queue: eventName,
                                      exchange: deadLetterExtrange,
                                      routingKey: eventName
                                      );

                    #endregion

                    var message = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(message);

                    policy.Execute(() =>
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2; // persistent
                        properties.ContentType = "application/json";
                        //properties.Type = "";

                        _logger.LogTrace("Publishing event to DeadLetter RabbitMQ: {EventId}", @event.Id);

                        channel.BasicPublish(
                            exchange: deadLetterExtrange,
                            routingKey: eventName,
                            mandatory: true,
                            basicProperties: properties,
                            body: body);
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Publishing DeadLetter event to RabbitMQ Error:", ex.ToString());

            }

        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subsManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueDeclare(queue: eventName,
                                          durable: true,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null
                                          );


                    channel.QueueBind(queue: eventName,
                                      exchange: _extrangeName,
                                      routingKey: eventName
                                      );
                }
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subsManager.Clear();
        }

        private void StartBasicConsume(string eventName)
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                if (_rabbitMQEventBusSettings.PrefetchCount > 0)
                {
                    _consumerChannel.BasicQos(0, _rabbitMQEventBusSettings.PrefetchCount, false);
                }

                var consumer = new EventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: eventName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private void StartBasicConsume()
        {
            if (!_subsManager.IsEmpty)
            {
                var eventNames = _subsManager.GetEventKeys();
                foreach (var eventName in eventNames)
                {
                    StartBasicConsume(eventName);
                }
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _subsManager.IsEmpty");
            }
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
                PublishToDeadLetter(new DeadLetterQueue()
                {
                    Message = message,
                    Queue = eventName,
                    CreateOn = DateTime.Now,
                    Exception = ex.ToString(),
                    Exchange = eventArgs.Exchange,
                    Id = Guid.NewGuid().ToString(),
                    RoutingKey = eventArgs.RoutingKey
                });
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _extrangeName, type: "direct", durable: true);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                        var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}
