using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Gateways;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Gateways
{
    public class ListInteractionsGatewayTests
    {
        private readonly InteractionsGateway _interactionsGateway;

        public ListInteractionsGatewayTests()
        {
            _interactionsGateway = new InteractionsGateway();
        }

        [Fact]
        public void ListOfInteractionsImplemetsBoundaryInterface()
        {
            Assert.True(_interactionsGateway is IInteractionsGateway);
        }

        [Fact]
        public void GetInteractionsByDateRangeReturnsEmptyList()
        {
            var response = _interactionsGateway.GetInteractionsByDateRange("1900-01-01", "1900-01-02");

            Assert.Empty(response);
            Assert.Null(response.FirstOrDefault());
        }

        [Fact]
        public void GetInteractionsByDateRangeReturnsAListOfInteractions()
        {
            var response = _interactionsGateway.GetInteractionsByDateRange("2019-04-01", "2019-04-30");

            Assert.IsType<List<Interaction>>(response);
            Assert.True(response.Count > 0);
        }

        [Fact]
        public void GetInteractionsAndChildInteractionsByDateRangeReturnsEmptyList()
        {
            var response = _interactionsGateway.GetInteractionsAndChildInteractionsByDateRange("1900-01-01", "1900-01-02");

            Assert.Empty(response);
            Assert.Null(response.FirstOrDefault());
        }
    }
}
