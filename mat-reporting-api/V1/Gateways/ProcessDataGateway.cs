using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Exceptions;
using MaTReportingAPI.V1.Factories;
using MaTReportingAPI.V1.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MaTReportingAPI.V1.Gateways
{
    public class ProcessDataGateway : IProcessDataGateway
    {
        private IProcessDbContext processDbContext;

        public ProcessDataGateway(IProcessDbContext _processDbContext)
        {
            processDbContext = _processDbContext;
        }

        public List<MatProcessData> GetProcessDataByIDs(string[] refs)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.In("_id", refs);
                var result = processDbContext.getCollection().FindAsync(filter).Result;

                List<MatProcessData> docs = new List<MatProcessData>();

                foreach (var doc in result.ToList())
                {
                    docs.Add(ProcessDataFactory.CreateProcessDataObject(doc));
                }

                return docs;
            }
            catch
            {
                throw new MaTProcessApiException("Unable to get home check details from process database");
            }
        }
    }
}
