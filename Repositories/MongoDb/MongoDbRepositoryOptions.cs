using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.MongoDb
{
    public class MongoDbRepositoryOptions : RepositoryOptions
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
