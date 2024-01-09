using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Infrastructure.HttpClient
{
    public static class HttpClientFactoryExtensions
    {
        public static IServiceCollection AddHttpClientFactory(this IServiceCollection services, IConfiguration Configuration)
        {
            var options = new HttpClientFactoryOptions();
            var optionList = new List<HttpClientFactoryOptions>();

            Configuration.GetSection(nameof(HttpClientFactoryOptions)).Bind(optionList);
            services.Configure<List<HttpClientFactoryOptions>>(Configuration.GetSection(nameof(HttpClientFactoryOptions)));

            foreach (var option in optionList)
            {
                services.AddHttpClient(option.HttpClientName, client =>
                {
                    client.BaseAddress = new Uri(option.BaseAddress);
                });
            }

            return services;
        }
    }
}
