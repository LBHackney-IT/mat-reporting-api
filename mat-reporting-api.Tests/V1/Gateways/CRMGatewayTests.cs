using MaTReportingAPI.Tests.V1.Helpers;
using MaTReportingAPI.V1.Exceptions;
using MaTReportingAPI.V1.Gateways;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Gateways
{
    public class CRMGatewayTests
    {
        private readonly CRMTokenGateway _mockCRMTokenGateway;
        private Uri mockBaseUri = new Uri("http://mockBase");

        public CRMGatewayTests()
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
        }

        [Fact]
        public void given_that_httpClient_returns_valid_response_then_gateway_returns_correct_value()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(InteractionsHelper.GetExpectedCRMGatewayResponseForGetInteractions())
            }).Verifiable();

            HttpClient httpCLient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = mockBaseUri
            };

            CRMGateway CRMGateway = new CRMGateway(_mockCRMTokenGateway, httpCLient);

            //act
            var result = CRMGateway.GetEntitiesByFetchXMLQuery("", "");

            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public void given_httpClient_returns_Unauthorized_response_then_gateway_throws_an_CRMException_exception()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized
            }).Verifiable();

            HttpClient httpCLient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = mockBaseUri
            };

            CRMGateway CRMGateway = new CRMGateway(_mockCRMTokenGateway, httpCLient);

            //assert
            Assert.Throws<CRMException>(delegate { CRMGateway.GetEntitiesByFetchXMLQuery("", ""); });
        }

    }
}
