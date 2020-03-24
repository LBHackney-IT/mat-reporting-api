using MongoDB.Bson;
using MongoDB.Driver;

namespace MaTReportingAPI.V1.Infrastructure
{
    public interface IProcessDbContext
    {
        IMongoCollection<BsonDocument> processCollection { get; set; }
        IMongoCollection<BsonDocument> getCollection();
    }
}
