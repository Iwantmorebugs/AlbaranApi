using MongoDB.Driver;

namespace AlbaranApi.Contracts
{
    public interface IMongoDatabaseFactory
    {
        IMongoDatabase Create();
    }
}