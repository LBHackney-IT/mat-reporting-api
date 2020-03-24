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
    public class CRMTokenGatewayTests
    {
        private Uri mockBaseUri = new Uri("http://mockBase");

        [Fact]
        public void given_httpClient_returns_valid_response_then_gateway_returns_access_token_string()
        {
            //arrange
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
                Content = new StringContent("{'result': 'fakeToken'}")
            }).Verifiable();

            HttpClient httpCLient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = mockBaseUri
            };

            CRMTokenGateway _CRMTokenGateway = new CRMTokenGateway(httpCLient);

            //act
            var response = _CRMTokenGateway.GetCRMAccessToken();

            //assert
            Assert.Equal("fakeToken", response);

        }

        [Fact]
        public void given_httpClient_returns_Unauthorized_response_then_gateway_throws_an_CRMTokenException_exception()
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

            CRMTokenGateway _CRMTokenGateway = new CRMTokenGateway(httpCLient);

            //assert
            Assert.Throws<CRMTokenException>(delegate { _CRMTokenGateway.GetCRMAccessToken(); } );
        }
    }
}
