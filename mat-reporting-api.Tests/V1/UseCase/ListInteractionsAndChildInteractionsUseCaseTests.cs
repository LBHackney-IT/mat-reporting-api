using MaTReportingAPI.UseCase.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Gateways;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MaTReportingAPI.Tests.V1.UseCase
{
    public class ListInteractionsAndChildInteractionsUseCaseTests
    {
        private readonly ListInteractionsAndChildInteractionsUseCase _interactionsAndChildInteractionsUseCase;
        private readonly Mock<IInteractionsGateway> _interactionsGateway;

        private readonly Interaction parentInteraction = Helpers.InteractionsHelper.GetInteraction();

        public ListInteractionsAndChildInteractionsUseCaseTests()
        {
            _interactionsGateway = new Mock<IInteractionsGateway>();
            _interactionsAndChildInteractionsUseCase = new ListInteractionsAndChildInteractionsUseCase(_interactionsGateway.Object);
        }

        [Fact]
        public void ListInteractionsAndChildInteractionsUseCaseImplementsBoundaryInterface()
        {
            Assert.True(_interactionsAndChildInteractionsUseCase is IListInteractionsAndChildInteractions);
        }

        [Fact]
        public void CanGetListOfInteractionsByDateRange()
        {
            ListInteractionsAndChildInteractionsRequest request = new ListInteractionsAndChildInteractionsRequest() { FromDate = "2019-04-01", ToDate = "2019-04-30" };

            List<Interaction> response = new List<Interaction>() { parentInteraction };

            _interactionsGateway.Setup(_ => _.GetInteractionsAndChildInteractionsByDateRange(request.FromDate, request.ToDate)).Returns(response);

            var results = _interactionsAndChildInteractionsUseCase.Execute(request);

            Assert.NotNull(results);
            Assert.True(results is ListInteractionsAndChildInteractionsResponse);
            Assert.True(results.Interactions.First() is Interaction);
            Assert.True(results.Request is ListInteractionsAndChildInteractionsRequest);

            Assert.Equal(response.FirstOrDefault().Name, results.Interactions.FirstOrDefault().Name);
        }

        [Fact]
        public void ExecuteReturnsResponseUsingsGatewayResults()
        {
            ListInteractionsAndChildInteractionsRequest request = new ListInteractionsAndChildInteractionsRequest() { FromDate = "2019-04-01", ToDate = "2019-04-30" };

            List<Interaction> response = new List<Interaction>() { parentInteraction };

            _interactionsGateway.Setup(_ => _.GetInteractionsAndChildInteractionsByDateRange(request.FromDate, request.ToDate)).Returns(response);

            _interactionsAndChildInteractionsUseCase.Execute(request);

            _interactionsGateway.Verify(gw => gw.GetInteractionsAndChildInteractionsByDateRange(request.FromDate, request.ToDate));
        }
    }
}
