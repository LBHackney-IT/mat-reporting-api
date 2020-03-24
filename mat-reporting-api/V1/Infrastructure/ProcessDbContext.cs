using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MaTReportingAPI.V1.Infrastructure
{
    public class ProcessDbContext : IProcessDbContext
    {
        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;
        public IMongoCollection<BsonDocument> processCollection { get; set; }

        public ProcessDbContext(IOptions<ConnectionSettings> appSettings)
        {
            string pathToCAFile = "/ssl/rds-ca-2019-root.pem";

            X509Store localTrustStore = new X509Store(StoreName.Root);
            string caContentString = System.IO.File.ReadAllText(pathToCAFile);

            X509Certificate2 caCert = new X509Certificate2(Encoding.ASCII.GetBytes(caContentString));

            try
            {
                localTrustStore.Open(OpenFlags.ReadWrite);
                localTrustStore.Add(caCert);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Root certificate import failed: " + ex.Message);
                throw;
            }
            finally
            {
                localTrustStore.Close();
            }

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
