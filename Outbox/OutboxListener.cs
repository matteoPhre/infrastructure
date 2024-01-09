using EasyNetQ;
using Infrastructure.Core.Events;
using Infrastructure.MessageBrokers;
using Infrastructure.Outbox.Stores;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Infrastructure.Outbox
{
    public class OutboxListener : IOutboxListener
    {
        private readonly IOutboxStore _store;

        public OutboxListener(IOutboxStore store)
        {
            _store = store;
        }

        public virtual async Task Commit(OutboxMessage message)
        {
            await _store.Add(message);
        }

        public virtual async Task Commit<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var outboxMessage = new OutboxMessage
            {
                AssemblyType = MessageBrokersHelper.GetTypeName<TEvent>(),
                Data = @event == null ? "{}" : JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }),
                EventType = @event.GetType().Name
            };

            await Commit(outboxMessage);
        }

        public virtual async Task CommitMessage<TEvent>(TEvent @event) where TEvent : IMessage
        {
            var outboxMessage = new OutboxMessage
            {
                AssemblyType = MessageBrokersHelper.GetTypeName<TEvent>(),
                Data = @event == null ? "{}" : JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }),
                EventType = @event.GetType().Name
            };

            await Commit(outboxMessage);
        }
    }
}
