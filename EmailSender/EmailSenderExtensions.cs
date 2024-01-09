using Infrastructure.EmailSender.Sender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.EmailSender
{
    public static class EmailSenderExtensions
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new EmailSenderOptions();
            configuration.GetSection(nameof(EmailSenderOptions)).Bind(options);

            services.Configure<EmailSenderOptions>(configuration.GetSection(nameof(EmailSenderOptions)));
            services.AddScoped<ISender, Sender.Sender>();

            return services;
        }
    }
}
