using MaTReportingAPI.UseCase.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Gateways;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MaTReportingAPI.Tests.V1.UseCase
{
    public class ListETRAMeetingsUseCaseTests
    {
        private readonly ListETRAMeetingsUseCase _eTRAMeetingsUseCase;
        private readonly Mock<IETRAMeetingsGateway> _eTRAMeetingsGateway;

        public ListETRAMeetingsUseCaseTests()
        {
            _eTRAMeetingsGateway = new Mock<IETRAMeetingsGateway>();
            _eTRAMeetingsUseCase = new ListETRAMeetingsUseCase(_eTRAMeetingsGateway.Object);
        }

        [Fact]
        public void ListETRAMeetingsUseCaseImplementsBoundaryInterface()
        {
            Assert.True(_eTRAMeetingsUseCase is IListETRAMeetings);
        }

        [Fact]
        public void CanGetListOfETRAMeetingsByDateRange()
        {
            ListETRAMeetingsRequest request = new ListETRAMeetingsRequest() { FromDate = "2019-04-01", ToDate = "2019-04-30" };

            List<ETRAMeeting> response = new List<ETRAMeeting>() { new ETRAMeeting() { Name = "TEST ETRA meeting" } };

            _eTRAMeetingsGateway.Setup(_ => _.GetETRAMeetingsByDateRange(request.FromDate, request.ToDate)).Returns(response);

            var results = _eTRAMeetingsUseCase.Execute(request);

            Assert.NotNull(results);
            Assert.True(results is ListETRAMeetingsResponse);
            Assert.True(results.ETRAMeetings.First() is ETRAMeeting);
            Assert.True(results.Request is ListETRAMeetingsRequest);

            Assert.Equal(response.FirstOrDefault().Name, results.ETRAMeetings.FirstOrDefault().Name);
        }

        [Fact]
        public void ExecuteReturnsResponseUsingsGatewayResults()
        {
            ListETRAMeetingsRequest request = new ListETRAMeetingsRequest() { FromDate = "2019-04-01", ToDate = "2019-04-30" };

            List<ETRAMeeting> response = new List<ETRAMeeting>() { new ETRAMeeting() { Name = "TEST ETRA meeting" } };

            _eTRAMeetingsGateway.Setup(_ => _.GetETRAMeetingsByDateRange(request.FromDate, request.ToDate)).Returns(response);

            var gwResponse = _eTRAMeetingsUseCase.Execute(request);

            _eTRAMeetingsGateway.Verify(gw => gw.GetETRAMeetingsByDateRange(request.FromDate, request.ToDate));
        }
    }
}
