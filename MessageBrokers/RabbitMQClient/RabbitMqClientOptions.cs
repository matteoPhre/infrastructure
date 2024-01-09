using EasyNetQ;

namespace Infrastructure.MessageBrokers.RabbitMQClient
{
    public class RabbitMqClientOptions : ConnectionConfiguration
    {
        public ExchangeOptions Exchange { get; set; }
        public QueueOptions Queue { get; set; }
        public string MessageBrokerType { get; set; }
    }

    public class ExchangeOptions
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Passive { get; set; }
        public bool AutoDelete { get; set; }
        public bool Internal { get; set; }
        public bool Delayed { get; set; }
        public string AlternateExchange { get; set; }
    }

    public class QueueOptions
    {
        public string Name { get; set; }
        public bool Passive { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
    }
}
