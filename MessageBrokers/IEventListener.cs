using EasyNetQ;
using Infrastructure.Core.Events;
using System;
using System.Threading.Tasks;

namespace Infrastructure.MessageBrokers
{
    public interface IEventListener
    {
        void Subscribe(Type type);
        void Subscribe<TEvent>() where TEvent : IEvent;
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
        Task PublishMessage<TEvent>(TEvent @event) where TEvent : IMessage;
        Task Publish(string message, string type);
    }
}
