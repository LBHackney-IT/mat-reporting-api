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
    public class ListInteractionsGatewayTests
    {
        private readonly InteractionsGateway _interactionsGateway;
        private readonly CRMTokenGateway _mockCRMTokenGateway;
        private readonly CRMGateway _mockCRMGateway;
        private readonly MaTProcessAPIGateway _mockProcessApiGateway;

        private Uri mockBaseUri = new Uri("http://mockBase");

        public ListInteractionsGatewayTests()
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
            _interactionsGateway = new InteractionsGateway(_mockCRMGateway, _mockProcessApiGateway);
        }

        [Fact]
        public void GetInteractionsByDateRangeReturnsAListOfInteractions()
        {
            //mock HttpMessageHandler for CRMGateway
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
   
            string expectedCRMGatewayResponseContent =
                Helpers.InteractionsHelper.GetExpectedCRMGatewayResponseForGetInteractions();

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

            //mock HttpMessageHandler for MaTAPIGAteway
            var httpMessageHandlerMockMaTAPI = new Mock<HttpMessageHandler>(MockBehavior.Strict);
           
            string expectedMaTAPIGatewayResponseContent =
                Helpers.InteractionsHelper.GetHomeCheckResults();

            httpMessageHandlerMockMaTAPI.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedMaTAPIGatewayResponseContent),
            }).Verifiable();

            HttpClient _matAPIClient = new HttpClient(httpMessageHandlerMockMaTAPI.Object) { BaseAddress = mockBaseUri };
            MaTProcessAPIGateway _mockMaTAPIGateway = new MaTProcessAPIGateway(_matAPIClient);

            //inject mock clients to gateway
            InteractionsGateway interactionsGateway = new InteractionsGateway(_mockCRMGateway, _mockMaTAPIGateway);

            //Act
            var response = interactionsGateway.GetInteractionsByDateRange("2019-04-01", "2019-04-30");

            //Assert
            Assert.NotNull(response);
            Assert.IsType<List<Interaction>>(response);
            Assert.NotNull(response.FirstOrDefault());
            Assert.True(response.Count > 0);
            //Assert.Equal(expectedMeetingName, response.FirstOrDefault().Name);
            //Assert.Equal(expectedNumberOfActions, response.FirstOrDefault().NoOfActions);
        }
    }
}
