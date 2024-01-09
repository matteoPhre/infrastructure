using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.MongoDb.Persistance
{
    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;

            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("MyContention", pack, t => true);
        }
    }
}
