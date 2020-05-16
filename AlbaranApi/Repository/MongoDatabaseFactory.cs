using AlbaranApi.Contracts;
using MongoDB.Driver;
using System;

namespace AlbaranApi.Repository
{
    public class MongoDatabaseFactory
        : IMongoDatabaseFactory
    {
        private readonly string _connectionString;

        public MongoDatabaseFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IMongoDatabase Create()
        {
            var mongoUrl = new MongoUrl(_connectionString);
            var databaseName = mongoUrl.DatabaseName;
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(databaseName);
            return database;
        }
    }
}
