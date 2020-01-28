using MaTReportingAPI.UseCase.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Gateways;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MaTReportingAPI.Tests.V1.UseCase
{
    public class ListInteractionsAndChildInteractionsUseCaseTests
    {
        private readonly ListInteractionsAndChildInteractionsUseCase _interactionsAndChildInteractionsUseCase;
        private readonly Mock<IInteractionsGateway> _interactionsGateway;

        private readonly ParentInteraction parentInteraction = new ParentInteraction()
        {
            AddressStreet1 = "Address Street 1",
            AddressStreet2 = "Address Street 2",
            AddressStreet3 = "Address Street 3",
            AddressZIPPostalCode = "E18",
            Contact = "Contact name",
            CreatedByEstateOfficer = "Created by estate officer",
            CreatedOn = "24/12/2019",
            EnquirySubject = "enquiry subject",
            EstateOfficerPatch = "estate officer patch",
            HandledBy = "Handled Officer",
            HomeCheck = "home check",
            HouseholdInteraction = "555",
            Id = "02d0ff4d-2555-e911-a555-002248072xyz",
            Incident = "incident type",
            ManagerPatch = "manager patch",
            Name = "CASE-NAME",
            NatureofEnquiry = "nature of enquiry",
            NHOAreaName = "NHO area name",
            ProcessStage = "555",
            ProcessType = "555",
            ReasonForStartingProcess = "reason for processing",
            Subject = "subject",
            Transferred = "transferred",
            UpdatedByEstateOfficer = "updated by"
        };

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

            List<ParentInteraction> response = new List<ParentInteraction>() { parentInteraction };

            _interactionsGateway.Setup(_ => _.GetInteractionsAndChildInteractionsByDateRange(request.FromDate, request.ToDate)).Returns(response);

            var results = _interactionsAndChildInteractionsUseCase.Execute(request);

            Assert.NotNull(results);
            Assert.True(results is ListInteractionsAndChildInteractionsResponse);
            Assert.True(results.Interactions.First() is ParentInteraction);
            Assert.True(results.Request is ListInteractionsAndChildInteractionsRequest);

            Assert.Equal(response.FirstOrDefault().Name, results.Interactions.FirstOrDefault().Name);
        }

        [Fact]
        public void ExecuteReturnsResponseUsingsGatewayResults()
        {
            ListInteractionsAndChildInteractionsRequest request = new ListInteractionsAndChildInteractionsRequest() { FromDate = "2019-04-01", ToDate = "2019-04-30" };

            List<ParentInteraction> response = new List<ParentInteraction>() { parentInteraction };

            _interactionsGateway.Setup(_ => _.GetInteractionsAndChildInteractionsByDateRange(request.FromDate, request.ToDate)).Returns(response);

            _interactionsAndChildInteractionsUseCase.Execute(request);

            _interactionsGateway.Verify(gw => gw.GetInteractionsAndChildInteractionsByDateRange(request.FromDate, request.ToDate));
        }
    }
}
