using Infrastructure.Repositories.MongoDb.Context;
using Infrastructure.Repositories.MongoDb.Interfaces;
using Infrastructure.Repositories.MongoDb.Persistance;
using Infrastructure.Repositories.MongoDb.UoW;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.MongoDb
{
    public static class MongoDbRepositoryExtensions
    {
        public static IServiceCollection AddMongoDbRepository(this IServiceCollection services, IConfiguration Configuration)
        {
            var options = new MongoDbRepositoryOptions();
            Configuration.GetSection(nameof(MongoDbRepositoryOptions)).Bind(options);
            services.Configure<MongoDbRepositoryOptions>(Configuration.GetSection(nameof(RepositoryOptions)));

            MongoDbPersistence.Configure();

            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
