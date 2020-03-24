using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Gateways;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Gateways
{
    public class ETRAMeetingsGatewayTests
    {
        private readonly Mock<CRMTokenGateway> mockCRMTokenGateway;
       
        private readonly Uri mockBaseUri = new Uri("http://mockBase");

        public ETRAMeetingsGatewayTests()
        {
            //common fake token to be used in all tests
            var mockHttpMessageHandlerCRMTokenGateway = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mockHttpMessageHandlerCRMTokenGateway.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{'result': 'fakeToken'}")
            }).Verifiable();

            HttpClient crmClient = new HttpClient(mockHttpMessageHandlerCRMTokenGateway.Object)
            {
                BaseAddress = mockBaseUri
            };

            mockCRMTokenGateway = new Mock<CRMTokenGateway>(crmClient);
        }

        [Fact]
        public void GetETRAMeetingsByDateRangeReturnsAListOfETRAMeetings()
        {
            //arrange
            //mock HttpMessageHandler, not HttpClient
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            string expectedMeetingName = "ETRA meeting";
            int expectedNumberOfActions = 1;

            string expectedCRMGatewayResponseContent =
                $@"{{""value"": [{{""hackney_name"": ""{expectedMeetingName}"",""NoOfActions"": ""{expectedNumberOfActions}""}}]}}";

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedCRMGatewayResponseContent),
            }).Verifiable();

            HttpClient client = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = mockBaseUri };
            Mock<CRMGateway> _mockCRMGateway = new Mock<CRMGateway>(mockCRMTokenGateway.Object, client);

            //inject mocks to gateway
            ETRAMeetingsGateway eTRAMeetingsGateway = new ETRAMeetingsGateway(_mockCRMGateway.Object);

            //Act
            var response = eTRAMeetingsGateway.GetETRAMeetingsByDateRange("2019-04-01", "2019-04-30");

            //Assert
            Assert.NotNull(response);
            Assert.IsType<List<ETRAMeeting>>(response);
            Assert.NotNull(response.FirstOrDefault());
            Assert.True(response.Count > 0);
            Assert.Equal(expectedMeetingName, response.FirstOrDefault().Name);
            Assert.Equal(expectedNumberOfActions, response.FirstOrDefault().NoOfActions);
        }
    }
}
