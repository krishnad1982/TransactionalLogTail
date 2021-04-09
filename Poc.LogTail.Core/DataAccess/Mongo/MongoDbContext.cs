using MongoDB.Driver;
using Poc.LogTail.Core.Configuration;

namespace Poc.LogTail.Core.DataAccess.Mongo
{
    public class MongoDbContext : IDbContext<IMongoDatabase>
    {
        private readonly IMongoDatabase _mongoDb;

        public MongoDbContext(Settings settings, IMongoClient mongoClient)
        {
            _mongoDb = mongoClient.GetDatabase(settings.MongoDbSettings.DatabaseName);
        }

        public IMongoDatabase GetDatabase()
        {
            return _mongoDb;
        }
    }

    public interface IDbContext<out T>
    {
        T GetDatabase();
    }
}
