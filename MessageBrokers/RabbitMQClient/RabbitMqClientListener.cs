using EasyNetQ;
using EasyNetQ.Topology;
using Infrastructure.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using IEventBus = Infrastructure.Core.Events.IEventBus;

namespace Infrastructure.MessageBrokers.RabbitMQClient
{
    public class RabbitMqClientListener : IEventListener
    {
        private readonly IAdvancedBus _busClient;
        private readonly IExchange _exchange;
        private readonly IServiceScopeFactory _serviceFactory;
        private readonly RabbitMqClientOptions _options;

        public RabbitMqClientListener(
            IAdvancedBus busClient,
            IExchange exchange,
            IOptions<RabbitMqClientOptions> options,
            IServiceScopeFactory serviceFactory)
        {
            _busClient = busClient;
            _exchange = exchange;
            _options = options.Value;
            _serviceFactory = serviceFactory;
        }

        //TODO unused
        public virtual async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event), "Event can not be null.");
            }
            await _busClient.PublishAsync(_exchange, @event.GetType().Name, true, new Message<TEvent>(@event));

        }

        //TODO unused
        public virtual async Task Publish(string message, string type)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message), "Event message can not be null.");
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type), "Event type can not be null.");
            }

            var toSend = new Message<string>(message);

            await _busClient.PublishAsync(_exchange, type, true, toSend);
        }
        public virtual async void Subscribe(Type type)
        {
            var queue = new Queue(MessageBrokersHelper.GetQueueName(_options.Queue.Name, type.Name), _options.Queue.Exclusive);

            var queueDeclared = await _busClient.QueueDeclareAsync(queue.Name, 
                passive: _options.Queue.Passive, 
                durable: _options.Queue.Durable, 
                exclusive: _options.Queue.Exclusive, 
                autoDelete: _options.Queue.AutoDelete);

            var bind = await _busClient.BindAsync(_exchange, queueDeclared, type.Name);

            _busClient.Consume<IEvent>(queueDeclared, async (msg, info)=> 
            {
                using (var scope = _serviceFactory.CreateScope())
                {
                    var eventBus = scope.ServiceProvider.GetService<IEventBus>();
                    await eventBus.PublishLocal(msg.Body);
                }
            });
        }

        public virtual void Subscribe<TEvent>() where TEvent : IEvent
        {
            Subscribe(typeof(TEvent));
        }

        public virtual async Task PublishMessage<TEvent>(TEvent @event) where TEvent : IMessage
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event), "Event can not be null.");
            }
            await _busClient.PublishAsync(_exchange, @event.MessageType.Name, true, @event);
        }
    }
}
