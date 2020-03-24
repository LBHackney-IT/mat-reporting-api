using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MaTReportingAPI.V1.Infrastructure
{
    public class ProcessDbContext : IProcessDbContext
    {
        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;
        public IMongoCollection<BsonDocument> processCollection { get; set; }

        public ProcessDbContext(IOptions<ConnectionSettings> appSettings)
        {
            mongoClient = new MongoClient(new MongoUrl(appSettings.Value.ConnectionString));
            //create a new blank database if database does not exist, otherwise get existing database
            mongoDatabase = mongoClient.GetDatabase(appSettings.Value.Database);
            //create collection to hold the documents if it does not exist, otherwise retrieve existing
            processCollection = mongoDatabase.GetCollection<BsonDocument>(appSettings.Value.CollectionName);
        }
        public IMongoCollection<BsonDocument> getCollection()
        {
            return processCollection;
        }
    }
}
