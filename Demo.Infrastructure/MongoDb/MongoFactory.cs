using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace Demo.Infrastructure.MongoDb
{
    public class MongoFactory
    {
        public IMongoDatabase Database { get; private set; }

        public MongoFactory(IConfiguration configuration)
        {
            var mongoContext = new MongoContext(configuration.GetSection("MongoSettings").GetValue<string>("MongoConnection"), configuration.GetSection("MongoSettings").GetValue<string>("MongoDbName"));

            Database = mongoContext.Database;
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>()
            where TDocument : class
        {
            return Database.GetCollection<TDocument>(typeof(TDocument).Name);
        }

        public bool CollectionExists(string name)
        {
            var filter = new BsonDocument("name", name);

            var collections = Database.ListCollections(new ListCollectionsOptions { Filter = filter });

            return (collections.ToList()).Any();
        }

        public void CreateCollection(string name)
        {
            Database.CreateCollection(name);
        }
    }
}
