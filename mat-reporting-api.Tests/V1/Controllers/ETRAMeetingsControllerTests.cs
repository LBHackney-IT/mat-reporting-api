using MaTReportingAPI.Controllers.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Domain;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Controllers
{
    public class ETRAMeetingsControllerTests
    {
        private readonly ETRAMeetingsController _eTRAMeetingsController;
        private readonly Mock<IListETRAMeetings> _mockListETRAMeetingsUseCase;

        private const string fromDate = "2019-04-01";
        private const string toDate = "2019-04-03";
        private const string meetingName = "ETRA meeting test";
        readonly DateTime generatedAtDateTime = DateTime.Now;

        readonly ListETRAMeetingsRequest request = new ListETRAMeetingsRequest() { FromDate = fromDate, ToDate = toDate };
        readonly List<ETRAMeeting> result = new List<ETRAMeeting>() { new ETRAMeeting() { Name = meetingName, NoOfActions = 1 } };

        public ETRAMeetingsControllerTests()
        {
            _mockListETRAMeetingsUseCase = new Mock<IListETRAMeetings>();
            _eTRAMeetingsController = new ETRAMeetingsController(_mockListETRAMeetingsUseCase.Object);
        }

        [Fact]
        public void ReturnsCorrectResponseWithStatus()
        {
            _mockListETRAMeetingsUseCase.Setup(s => s.Execute(It.IsAny<ListETRAMeetingsRequest>()))
                .Returns(new ListETRAMeetingsResponse(result, request, generatedAtDateTime, 10, 5));

            var response = _eTRAMeetingsController.GetETRAMeetingsByDateRange(request);

            var json = JsonConvert.SerializeObject(response.Value);

            Assert.Equal(200, response.StatusCode);

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new Dictionary<string, object>
                    {
                        {
                            "request",
                            new Dictionary<string, object>
                              {
                                { "fromDate", fromDate },
                                { "toDate", toDate }
                              }
                        },
                        { "generatedAt", generatedAtDateTime },
                        { "totalNoOfMeetings", 10 },
                        { "totalNoOfActions", 5 },
                        { "etraMeetings",
                            new [] {
                                new Dictionary<string, object>
                                {
                                    { "name",  meetingName },
                                    { "noOfActions", 1 }
                                }
                            }
                        }
                    }
                ), json);
        }
    }
}
