using Infrastructure.Repositories.MongoDb.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.MongoDb.Context
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoClient MongoClient { get; set; }

        private readonly List<Func<Task>> _commands;
        
        private readonly MongoDbRepositoryOptions _configuration;

        public MongoContext(IOptions<MongoDbRepositoryOptions> configuration)
        {
            _configuration = configuration.Value;
            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Func<Task>>();
        }


        public async Task CreateCollection(string typeName)
        {
            await Database.CreateCollectionAsync(typeName);
        }


        public async Task<int> SaveChanges()
        {
            ConfigureMongo();

            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            return _commands.Count;
        }

        private void ConfigureMongo()
        {
            if (MongoClient != null)
                return;
            // Configure mongo (You can inject the config, just to simplify)

            var settings = new MongoClientSettings
            {
                WriteEncoding = new UTF8Encoding(false, false),
                ReadEncoding = new UTF8Encoding(false, false),
                Server = MongoServerAddress.Parse(_configuration.Host),
                Credential = MongoCredential.CreateCredential("admin", _configuration.Username, _configuration.Password),
            };

            MongoClient = new MongoClient(settings);
            Database = MongoClient.GetDatabase(_configuration.DatabaseName);

        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();
            return Database.GetCollection<T>(name);
        }

        public async Task AbortTransaction()
        {
            using (Session = await MongoClient.StartSessionAsync())
            {
                await Session.AbortTransactionAsync();
                _commands.Clear();
            }
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }
    }
}
