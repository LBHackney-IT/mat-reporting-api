using MaTReportingAPI.Tests.V1.Helpers;
using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using System;
using System.Linq;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Infrastructure
{
    public class ProcessDbContextTests
    {
        private Mock<IOptions<ConnectionSettings>> mockOptions;
        protected IMongoCollection<BsonDocument> collection;
        protected IMongoDatabase mongoDatabase;

        public ProcessDbContextTests()
        {
            mockOptions = new Mock<IOptions<ConnectionSettings>>();

            string MONGO_CONN_STRING = Environment.GetEnvironmentVariable("MONGO_CONN_STRING") ??
                              @"mongodb://localhost:1433/";

            //runtime DB connection strings
            var settings = new ConnectionSettings
            {
                ConnectionString = MONGO_CONN_STRING,
                CollectionName = "process-data",
                Database = "mat-processes"
            };
            mockOptions.SetupGet(x => x.Value).Returns(settings);

            MongoClient mongoClient = new MongoClient(new MongoUrl(MONGO_CONN_STRING));
            mongoDatabase = mongoClient.GetDatabase("mat-processes");
            collection = mongoDatabase.GetCollection<BsonDocument>("process-data");
        }

        [Fact]
        public void Context_can_get_process_data()
        {
            //arrange
            MatProcessData processData = ProcessDataHelper.CreateProcessDataObject();
            var bsonObject = BsonDocument.Parse(JsonConvert.SerializeObject(processData));
            collection.InsertOne(bsonObject);
            //act
            var context = new ProcessDbContext(mockOptions.Object);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", processData.Id);
            var result = context.getCollection().FindAsync(filter).Result.FirstOrDefault();
            var resultDeserialized = BsonSerializer.Deserialize<MatProcessData>(result);
            //assert
            Assert.Equal(processData.Id, resultDeserialized.Id);
            Assert.Equal(processData.ProcessType.value, resultDeserialized.ProcessType.value);
            Assert.Equal(processData.ProcessType.name, resultDeserialized.ProcessType.name);
            Assert.Equal(processData.ProcessDataAvailable, resultDeserialized.ProcessDataAvailable);
            Assert.Equal(processData.ProcessDataSchemaVersion, resultDeserialized.ProcessDataSchemaVersion);
            Assert.Equal(processData.ProcessStage, resultDeserialized.ProcessStage);
            Assert.Equal(processData.LinkedProcessId, resultDeserialized.LinkedProcessId);
            Assert.Equal(processData.PreProcessData, resultDeserialized.PreProcessData);
            Assert.Equal(processData.ProcessData, resultDeserialized.ProcessData);
            Assert.Equal(processData.PostProcessData, resultDeserialized.PostProcessData);
        }
    }
}
