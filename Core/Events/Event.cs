using EasyNetQ;
using System;

namespace Infrastructure.Core.Events
{
    public abstract class Event : IEvent
    {
        public virtual Guid Id { get; } = Guid.NewGuid();
        public virtual DateTime CreatedUtc { get; } = DateTime.UtcNow;
        public MessageProperties Properties { get; set; }
        public Type MessageType { get; set; }

        public object GetBody()
        {
            throw new NotImplementedException();
        }
    }
}
