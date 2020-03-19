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
        private readonly CRMTokenGateway _mockCRMTokenGateway;
        private readonly CRMGateway _mockCRMGateway;
        private readonly ETRAMeetingsGateway _eTRAMeetingsGateway;

        private Uri mockBaseUri = new Uri("http://mockBase");

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

            _mockCRMTokenGateway = new CRMTokenGateway(crmClient);

            //mock CRM
            _eTRAMeetingsGateway = new ETRAMeetingsGateway(_mockCRMGateway);
        }

        [Fact]
        public void ListOfETRAMeetingsImplemetsBoundaryInterface()
        {
            Assert.True(_eTRAMeetingsGateway is IETRAMeetingsGateway);
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
            CRMGateway _mockCRMGateway = new CRMGateway(_mockCRMTokenGateway, client);

            //inject mock clients to gateway
            ETRAMeetingsGateway eTRAMeetingsGateway = new ETRAMeetingsGateway(_mockCRMGateway);

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
