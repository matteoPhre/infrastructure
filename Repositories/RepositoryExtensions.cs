using Elastic.Apm.Api;
using Infrastructure.Repositories.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RepositoryOptions();
            configuration.GetSection(nameof(RepositoryOptions)).Bind(options);
            services.Configure<RepositoryOptions>(configuration.GetSection(nameof(RepositoryOptions)));

            switch (options.RepositoryType.ToLowerInvariant())
            {
                case "mongo":
                case "mongodb":
                    services.AddMongoDbRepository(configuration);
                    break;
                default:
                    break;
            }

            return services;
        }
    }
}
