using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Demo.Infrastructure.MongoDb
{
    public class MongoContext : MongoClient
    {
        public IMongoDatabase Database { get; set; }

        public MongoContext(string connectionString, string dbName) : base(connectionString)
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Register("CamelCaseConvensions", pack, t => true);

            Database = GetDatabase(dbName);
        }
    }
}
