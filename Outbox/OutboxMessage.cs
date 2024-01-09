using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Infrastructure.Outbox
{
    public class OutboxMessage
    {
        public OutboxMessage()
        {}
        public OutboxMessage(Guid id)
        {
            Id = id;
        }
        [BsonId]
        public Guid Id { get; private set; }
        public DateTime CreatedUtc { get; } = DateTime.UtcNow;
        public string AssemblyType { get; set; }

        public string EventType { get; set; }
        public string Data { get; set; }
        public DateTime? Processed { get; set; }
    }
}
