using API_Adresse.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SMART_Pressing.DataAccessLayer.DbContext
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Address> Addresses => _database.GetCollection<Address>("addresses");

    }
}
