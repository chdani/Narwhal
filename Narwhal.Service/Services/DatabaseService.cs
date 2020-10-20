using System;
using System.Linq;

using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Narwhal.Service.Services
{
    public class DatabaseService
    {
        private readonly ILogger<DatabaseService> _logger;

        private MongoClient mongoClient;

        public DatabaseService(ILogger<DatabaseService> logger)
        {
            _logger = logger;

            string databaseHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
            if (string.IsNullOrEmpty(databaseHost))
                databaseHost = "127.0.0.1";

            string databasePortText = Environment.GetEnvironmentVariable("DATABASE_PORT");
            if (string.IsNullOrEmpty(databasePortText) || !int.TryParse(databasePortText, out int databasePort))
                databasePort = 27017;

            _logger.LogInformation($"Connecting to MongoDB on {databaseHost}:{databasePort}");

            // Create client and force connection
            try
            {
                MongoClientSettings mongoClientSettings = new MongoClientSettings()
                {
                    Server = new MongoServerAddress(databaseHost, databasePort),
                    ServerSelectionTimeout = TimeSpan.FromSeconds(3)
                };

                mongoClient = new MongoClient(mongoClientSettings);
                mongoClient.ListDatabaseNames().First();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not connect to MongoDB");
                throw;
            }

            _logger.LogInformation("Connection successful");
        }

        public IMongoCollection<BsonDocument> GetNavWarningCollection() => mongoClient.GetDatabase("narwhal").GetCollection<BsonDocument>("navwarnings");
        public IMongoCollection<BsonDocument> GetTrackingCollection() => mongoClient.GetDatabase("narwhal").GetCollection<BsonDocument>("tracking");
        public IMongoCollection<BsonDocument> GetEventsCollection() => mongoClient.GetDatabase("narwhal").GetCollection<BsonDocument>("events");
    }
}
