using MaTReportingAPI.V1.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MaTReportingAPI.V1.Factories
{
    public class ProcessDataFactory
    {
        public static MatProcessData CreateProcessDataObject(BsonDocument bsonResult)
        {
            if (bsonResult != null)
            {
                return BsonSerializer.Deserialize<MatProcessData>(bsonResult);
            }
            else
            {
                return new MatProcessData();
            }
        }
    }
}
