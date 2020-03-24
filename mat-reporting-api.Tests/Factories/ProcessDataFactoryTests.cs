using MaTReportingAPI.Tests.V1.Helpers;
using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Factories;
using MongoDB.Bson;
using Newtonsoft.Json;
using Xunit;

namespace MaTReportingAPI.Tests.Factories
{
    public class ProcessDataFactoryTests
    {

        [Fact]
        public void can_create_mat_process_data_object_from_empty_object()
        {
            //arrange
            var matProcessData = new MatProcessData();
            //act
            var processDataFromFactory = ProcessDataFactory.CreateProcessDataObject(BsonDocument.Parse(JsonConvert.SerializeObject(matProcessData)));
            //assert
            Assert.Equal(matProcessData.ProcessDataSchemaVersion, processDataFromFactory.ProcessDataSchemaVersion);
            Assert.Equal(matProcessData.DateCompleted, processDataFromFactory.DateCompleted);
            Assert.Equal(matProcessData.DateCreated, processDataFromFactory.DateCreated);
            Assert.Equal(matProcessData.DateLastModified, processDataFromFactory.DateLastModified);
            Assert.Equal(matProcessData.Id, processDataFromFactory.Id);
            Assert.Equal(matProcessData.PostProcessData, processDataFromFactory.PostProcessData);
            Assert.Equal(matProcessData.PreProcessData, processDataFromFactory.PreProcessData);
            Assert.Equal(matProcessData.ProcessData, processDataFromFactory.ProcessData);
            Assert.Equal(matProcessData.ProcessStage, processDataFromFactory.ProcessStage);
            Assert.Equal(matProcessData.ProcessType, processDataFromFactory.ProcessType);
        }

        [Fact]
        public void can_create_mat_process_data_object_from_populated_object()
        {
            //arrange
            var matProcessData = ProcessDataHelper.CreateProcessDataObject();
            //act
            var processDataFromFactory = ProcessDataFactory.CreateProcessDataObject(BsonDocument.Parse(JsonConvert.SerializeObject(matProcessData)));
            //assert
            Assert.Equal(matProcessData.Id, processDataFromFactory.Id);
            Assert.Equal(matProcessData.ProcessType.value, processDataFromFactory.ProcessType.value);
            Assert.Equal(matProcessData.ProcessType.name, processDataFromFactory.ProcessType.name);
            Assert.Equal(matProcessData.DateCreated, processDataFromFactory.DateCreated);
            Assert.Equal(matProcessData.DateLastModified, processDataFromFactory.DateLastModified);
            Assert.Equal(matProcessData.DateCompleted, processDataFromFactory.DateCompleted);
            Assert.Equal(matProcessData.ProcessDataAvailable, processDataFromFactory.ProcessDataAvailable);
            Assert.Equal(matProcessData.ProcessDataSchemaVersion, processDataFromFactory.ProcessDataSchemaVersion);
            Assert.Equal(matProcessData.ProcessStage, processDataFromFactory.ProcessStage);
            Assert.Equal(matProcessData.LinkedProcessId, processDataFromFactory.LinkedProcessId);
            Assert.Equal(matProcessData.PreProcessData, processDataFromFactory.PreProcessData);
            Assert.Equal(matProcessData.ProcessData, processDataFromFactory.ProcessData);
            Assert.Equal(matProcessData.PostProcessData, processDataFromFactory.PostProcessData);
        }
    }
}
