using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace MaTReportingAPI.V1.Domain
{
    [BsonIgnoreExtraElements]
    public class MatProcessData
    {
        [JsonProperty("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty("processType")]
        [BsonElement("processType")]
        public ProcessType ProcessType { get; set; }

        [JsonProperty("dateCreated")]
        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("dateLastModified")]
        [BsonElement("dateLastModified")]
        public DateTime DateLastModified { get; set; }

        [JsonProperty("dateCompleted")]
        [BsonElement("dateCompleted")]
        public DateTime DateCompleted { get; set; }

        [JsonProperty("processDataAvailable")]
        [BsonElement("processDataAvailable")]
        public bool ProcessDataAvailable { get; set; }

        [JsonProperty("dataSchemaVersion")]
        [BsonElement("dataSchemaVersion")]
        public int ProcessDataSchemaVersion { get; set; }

        [JsonProperty("processStage")]
        [BsonElement("processStage")]
        public string ProcessStage { get; set; }

        [JsonProperty("linkedProcessId")]
        [BsonElement("linkedProcessId")]
        public string LinkedProcessId { get; set; }

        [JsonProperty("preProcessData")]
        [BsonElement("preProcessData")]
        public object PreProcessData { get; set; }

        [JsonProperty("processData")]
        [BsonElement("processData")]
        public object ProcessData { get; set; }

        [JsonProperty("postProcessData")]
        [BsonElement("postProcessData")]
        public object PostProcessData { get; set; }
    }

    public class ProcessType
    {
        public int value { get; set; }
        public string name { get; set; }
    }
}
