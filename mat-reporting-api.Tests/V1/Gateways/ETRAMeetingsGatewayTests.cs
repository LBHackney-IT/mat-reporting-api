using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Gateways;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Gateways
{
    public class ETRAMeetingsGatewayTests
    {
        private readonly ETRAMeetingsGateway _eTRAMeetingsGateway;

        public ETRAMeetingsGatewayTests()
        {
            _eTRAMeetingsGateway = new ETRAMeetingsGateway();
        }

        [Fact]
        public void ListOfETRAMeetingsImplemetsBoundaryInterface()
        {
            Assert.True(_eTRAMeetingsGateway is IETRAMeetingsGateway);
        }

        [Fact]
        public void GetETRAMeetingsByDateRangeReturnsEmptyList()
        {
            var response = _eTRAMeetingsGateway.GetETRAMeetingsByDateRange("1900-01-01", "1900-01-02");

            Assert.Empty(response);
            Assert.Null(response.FirstOrDefault());
        }

        [Fact]
        public void GetETRAMeetingsByDateRangeReturnsAListOfETRAMeetings()
        {
            var response = _eTRAMeetingsGateway.GetETRAMeetingsByDateRange("2019-04-01", "2019-04-30");

            Assert.IsType<List<ETRAMeeting>>(response);
            Assert.True(response.Count > 0);
        }
    }
}
