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

        private readonly Interaction intercation = new Interaction()
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
