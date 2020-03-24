using Bogus;
using MaTReportingAPI.V1.Domain;

namespace MaTReportingAPI.Tests.V1.Helpers
{
    public static class ProcessDataHelper
    {
        private static Faker faker = new Faker();
        
        public static MatProcessData CreateProcessDataObject()
        {
            return new MatProcessData
            {
                Id = faker.Random.Guid().ToString(),
                ProcessType = new ProcessType()
                {
                    value = faker.Random.Int(),
                    name = faker.Random.Word()
                },
                DateCreated = faker.Date.Recent(),
                DateLastModified = faker.Date.Recent(),
                DateCompleted = faker.Date.Recent(),
                ProcessDataAvailable = false,
                ProcessDataSchemaVersion = faker.Random.Int(1, 10),
                ProcessStage = "Not completed",
                LinkedProcessId = null,
                PreProcessData = { },
                ProcessData = { },
                PostProcessData = { },
            };
        }
    }
}
