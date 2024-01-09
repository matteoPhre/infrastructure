using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Logging
{
    public static class LoggingExtensions
    {
        public static Serilog.Core.Logger AddLogging(IConfiguration Configuration, string namespaceValue)
        {            
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithCorrelationIdHeader()
                .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger")))
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(Configuration["ElasticConfiguration:Uri"]))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{namespaceValue}-{DateTime.UtcNow:yyyy-MM}"
                })
            .CreateLogger();

            return logger;
        }

        public static IApplicationBuilder UseLogging(this IApplicationBuilder app, IConfiguration Configuration, ILoggerFactory loggerFactory)
        {
            app.UseAllElasticApm(Configuration);

            loggerFactory.AddSerilog();

            return app;
        }
    }
}
