using MaTReportingAPI.Controllers.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.CustomExceptions;
using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Validators;
using Microsoft.AspNetCore.Mvc;
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
        
        private const string fromDate = "2019-40-01";
        private const string toDate = "2019-04-03";
        private const string meetingName = "ETRA meeting test";
        readonly DateTime generatedAtDateTime = DateTime.Now;

        readonly ListETRAMeetingsRequest request = new ListETRAMeetingsRequest() { FromDate = fromDate, ToDate = toDate };
        readonly List<ETRAMeeting> meetings = new List<ETRAMeeting>() { new ETRAMeeting() { Name = meetingName, NoOfActions = 1 } };

        public ETRAMeetingsControllerTests()
        {
            _mockListETRAMeetingsUseCase = new Mock<IListETRAMeetings>();
            _eTRAMeetingsController = new ETRAMeetingsController(_mockListETRAMeetingsUseCase.Object);
        }

        [Fact]
        public void ReturnsCorrectResponseWithStatus()
        {
            _mockListETRAMeetingsUseCase.Setup(s => s.Execute(It.IsAny<ListETRAMeetingsRequest>()))
                .Returns(new ListETRAMeetingsResponse(meetings, request, generatedAtDateTime, 10, 5));

            var response = _eTRAMeetingsController.GetETRAMeetingsByDateRange(request);

            var result = (ObjectResult)(response);

            var json = JsonConvert.SerializeObject(result.Value);

            Assert.Equal(200, result.StatusCode);

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

        [Fact]
        public void Given_that_useCase_throws_an_exception_then_controller_returns_status_code_500()
        {
            //Arrange

            _mockListETRAMeetingsUseCase.Setup(x => x.Execute(It.IsAny<ListETRAMeetingsRequest>())).Throws<Exception>();

            //Act
            var response = _eTRAMeetingsController.GetETRAMeetingsByDateRange(request);

            var result = (StatusCodeResult)response;

            //Assert
            Assert.Equal(500, result.StatusCode);
        }

        
    }
}
