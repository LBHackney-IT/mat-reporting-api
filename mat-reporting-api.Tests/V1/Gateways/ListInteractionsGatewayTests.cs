using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Gateways;
using MaTReportingAPI.V1.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
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
        private Mock<IOptions<ConnectionSettings>> mockOptions;
        private readonly Mock<CRMTokenGateway> mockCRMTokenGateway;
        private readonly Mock<ProcessDataGateway> mockProcessDataGateway;
        private readonly Mock<MaTProcessDataGateway> mockMaTProcessDataGateway;
        protected IMongoCollection<BsonDocument> collection;
        protected IMongoDatabase mongoDatabase;
        private Mock<IProcessDbContext> mockContext;

        private Uri mockBaseUri = new Uri("http://mockBase");

        public ListInteractionsGatewayTests()
        {
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

            //TODO: Mock collection
            mockOptions = new Mock<IOptions<ConnectionSettings>>();

            string MONGO_CONN_STRING = Environment.GetEnvironmentVariable("MONGO_CONN_STRING") ??
                              @"mongodb://localhost:1433/";

            var settings = new ConnectionSettings
            {
                ConnectionString = MONGO_CONN_STRING,
                CollectionName = "process-data",
                Database = "mat-processes"
            };

            mockOptions.SetupGet(x => x.Value).Returns(settings);

            MongoClient mongoClient = new MongoClient(new MongoUrl(MONGO_CONN_STRING));
            mongoDatabase = mongoClient.GetDatabase("mat-processes");
            collection = mongoDatabase.GetCollection<BsonDocument>("process-data");

            mockContext = new Mock<IProcessDbContext>();
            mockContext.Setup(x => x.getCollection()).Returns(collection);

            mockProcessDataGateway = new Mock<ProcessDataGateway>(mockContext.Object);

            mockMaTProcessDataGateway = new Mock<MaTProcessDataGateway>();
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
            Mock<CRMGateway> _mockCRMGateway = new Mock<CRMGateway>(mockCRMTokenGateway.Object, client);

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
            Mock<MaTProcessAPIGateway> _mockMaTAPIGateway = new Mock<MaTProcessAPIGateway>(_matAPIClient);

            //inject mocks to gateway
            InteractionsGateway interactionsGateway = new InteractionsGateway(_mockCRMGateway.Object, _mockMaTAPIGateway.Object, mockProcessDataGateway.Object, mockMaTProcessDataGateway.Object);

            //Act
            var response = interactionsGateway.GetInteractionsByDateRange("2019-04-01", "2019-04-30");

            //Assert
            Assert.NotNull(response);
            Assert.IsType<List<Interaction>>(response);
            Assert.NotNull(response.FirstOrDefault());
            Assert.True(response.Count > 0);
        }
    }
}
