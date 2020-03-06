using MaTReportingAPI.UseCase.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Gateways;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MaTReportingAPI.Tests.V1.UseCase
{
    public class ListInteractionsUseCaseTests
    {
        private readonly ListInteractionsUseCase _intercationsUseCase;
        private readonly Mock<IInteractionsGateway> _interactionsGateway;

        private readonly Interaction intercation = Helpers.InteractionsHelper.GetInteraction();

        public ListInteractionsUseCaseTests()
        {
            _interactionsGateway = new Mock<IInteractionsGateway>();
            _intercationsUseCase = new ListInteractionsUseCase(_interactionsGateway.Object);
        }

        [Fact]
        public void ListInteractionsUseCaseImplementsBoundaryInterface()
        {
            Assert.True(_intercationsUseCase is IListInteractions);
        }

        [Fact]
        public void CanGetListOfInteractionsByDateRange()
        {
            ListInteractionsRequest request = new ListInteractionsRequest() { FromDate = "2019-04-01", ToDate = "2019-04-30" };

            List<Interaction> response = new List<Interaction>() { intercation };

            _interactionsGateway.Setup(_ => _.GetInteractionsByDateRange(request.FromDate, request.ToDate)).Returns(response);

            var results = _intercationsUseCase.Execute(request);

            Assert.NotNull(results);
            Assert.True(results is ListInteractionsResponse);
            Assert.True(results.Interactions.First() is Interaction);
            Assert.True(results.Request is ListInteractionsRequest);

            Assert.Equal(response.FirstOrDefault().Name, results.Interactions.FirstOrDefault().Name);
        }

        [Fact]
        public void ExecuteReturnsResponseUsingsGatewayResults()
        {
            ListInteractionsRequest request = new ListInteractionsRequest() { FromDate = "2019-04-01", ToDate = "2019-04-30" };

            List<Interaction> response = new List<Interaction>() { intercation} ;

            _interactionsGateway.Setup(_ => _.GetInteractionsByDateRange(request.FromDate, request.ToDate)).Returns(response);

            _intercationsUseCase.Execute(request);

            _interactionsGateway.Verify(gw => gw.GetInteractionsByDateRange(request.FromDate, request.ToDate));
        }
    }
}
