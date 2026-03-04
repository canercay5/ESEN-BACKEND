using ESEN.Domain.Entities;
using MongoDB.Driver;

namespace ESEN.Infrastructure.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoDatabase Database => _database;
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Region> Regions => _database.GetCollection<Region>("Regions");
        public IMongoCollection<OutbreakAlert> OutbreakAlerts => _database.GetCollection<OutbreakAlert>("OutbreakAlerts");
        public IMongoCollection<PushNotification> PushNotifications => _database.GetCollection<PushNotification>("PushNotifications");
        public IMongoCollection<DailyHealthMetric> DailyHealthMetrics => _database.GetCollection<DailyHealthMetric>("DailyHealthMetrics");
    }
}