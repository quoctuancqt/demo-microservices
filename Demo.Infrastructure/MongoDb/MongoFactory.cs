using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Demo.Infrastructure.MongoDb
{
    public class MongoFactory
    {
        private readonly MongoContext _mongoContext;
        public IMongoDatabase Database { get; private set; }

        public MongoFactory(IConfiguration configuration)
        {
            _mongoContext = new MongoContext(configuration.GetSection("MongoSettings").GetValue<string>("MongoConnection"), configuration.GetSection("MongoSettings").GetValue<string>("MongoDbName"));

            Database = _mongoContext.Database;
        }
    }
}
