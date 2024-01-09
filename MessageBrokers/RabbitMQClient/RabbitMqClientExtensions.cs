using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MessageBrokers.RabbitMQClient
{
    public static class RabbitMqClientExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration Configuration)
        {
            var options = new RabbitMqClientOptions();
            Configuration.GetSection(nameof(MessageBrokersOptions)).Bind(options);
            Configuration.GetSection("MessageBrokersOptions:Hosts").Bind(options.Hosts);
            services.Configure<RabbitMqClientOptions>(Configuration.GetSection(nameof(MessageBrokersOptions)));
            
            var advancedBus = RabbitHutch.CreateBus(options, opt =>
            {
                opt.Register<IEventListener, RabbitMqClientListener>();
            }).Advanced;

            var exchange = advancedBus.ExchangeDeclare(options.Exchange.Name, 
                options.Exchange.Type,
                passive: options.Exchange.Passive, 
                options.Exchange.AutoDelete, 
                @internal: options.Exchange.Internal, 
                alternateExchange: string.IsNullOrEmpty(options.Exchange.AlternateExchange) ? null : options.Exchange.AlternateExchange, 
                delayed: options.Exchange.Delayed);

            services.AddSingleton(advancedBus);
            services.AddSingleton(exchange);
            services.AddSingleton<IEventListener, RabbitMqClientListener>();

            return services;
        }
    }
}
