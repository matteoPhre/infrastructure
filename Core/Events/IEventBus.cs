using EasyNetQ;
using Infrastructure.EventStores;
using System.Threading.Tasks;

namespace Infrastructure.Core.Events
{
    public interface IEventBus
    {
        Task PublishLocal(params IEvent[] events);
        Task Commit(params IEvent[] events);

        Task PublishLocal(params IMessage[] events);
        Task Commit(params IMessage[] events);
        Task Commit(StreamState stream);
    }
}
