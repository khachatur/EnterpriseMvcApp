using EnterpriseMvcApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EnterpriseMvcApp.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Content> Contents => _database.GetCollection<Content>("Contents");
        public IMongoCollection<ContentVersion> ContentVersions => _database.GetCollection<ContentVersion>("ContentVersions");
    }
}
