using MaTReportingAPI.Controllers.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Domain;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Controllers
{
    public class InteractionsControllerTests
    {
        private readonly InteractionsController interactionsController;
        private readonly Mock<IListInteractions> mockListInteractionsUseCase;
        private readonly Mock<IListInteractionsAndChildInteractions> mockListInteractionsAndChildInteractionsUseCase;

        private const string fromDate = "2019-04-01";
        private const string toDate = "2019-04-03";

        private readonly ListInteractionsRequest listInteractionsRequest = new ListInteractionsRequest() { FromDate = fromDate, ToDate = toDate };
        private readonly ListInteractionsAndChildInteractionsRequest listInteractionsAndChildInteractionsRequest = new ListInteractionsAndChildInteractionsRequest() { FromDate = fromDate, ToDate = toDate };

        private readonly DateTime generatedAt = DateTime.Now;

        private static Interaction interaction = new Interaction()
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
            UpdatedByEstateOfficer = "updated by",
            ParentInteractionId = "a4b44f8e-e714-ea11-a811-000d3a86d68d"
        };

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
            UpdatedByEstateOfficer = "updated by",
            ParentInteractionId = null,
            ChildInteractions = new List<Interaction>() { interaction }
        };

        private List<Interaction> interactions = new List<Interaction>();
        private List<ParentInteraction> interactionsAndChildInteraction = new List<ParentInteraction>();

        public InteractionsControllerTests()
        {
            mockListInteractionsUseCase = new Mock<IListInteractions>();
            mockListInteractionsAndChildInteractionsUseCase = new Mock<IListInteractionsAndChildInteractions>();

            interactionsController = new InteractionsController(
                mockListInteractionsUseCase.Object,
                mockListInteractionsAndChildInteractionsUseCase.Object);

            interactions.Add(interaction);
            interactionsAndChildInteraction.Add(parentInteraction);
        }

        [Fact]
        public void GetInteractionsByDateRangeReturnsCorrectResponseWithStatus()
        {
            mockListInteractionsUseCase.Setup(_ => _.Execute(It.IsAny<ListInteractionsRequest>()))
                .Returns(new ListInteractionsResponse(listInteractionsRequest, generatedAt, interactions));

            var response = interactionsController.GetInteractionsByDateRange(listInteractionsRequest);
            var json = JsonConvert.SerializeObject(response.Value);
           
            Assert.Equal(200, response.StatusCode);

            Assert.Equal(
               JsonConvert.SerializeObject(
                   new Dictionary<string, object>
                   {
                        {
                            "request",
                            new Dictionary<string, object>
                              {
                                { "fromDate", fromDate },
                                { "toDate", toDate }
                              }
                        },
                        { "generatedAt", generatedAt },
                        { "interactions",
                            new [] {
                                new Dictionary<string, object>
                                {
                                    {"id","02d0ff4d-2555-e911-a555-002248072xyz"},
                                    {"parentInteractionId", "a4b44f8e-e714-ea11-a811-000d3a86d68d" },
                                    {"name","CASE-NAME"},
                                    {"createdOn","24/12/2019"},
                                    {"contact","Contact name"},
                                    {"natureofEnquiry","nature of enquiry"},
                                    {"createdByEstateOfficer","Created by estate officer"},
                                    {"enquirySubject","enquiry subject"},
                                    {"estateOfficerPatch","estate officer patch"},
                                    {"handledBy","Handled Officer"},
                                    {"householdInteraction","555"},
                                    {"incident","incident type"},
                                    {"managerPatch","manager patch"},
                                    {"nhoAreaName","NHO area name"},
                                    {"processStage","555"},
                                    {"processType","555"},
                                    {"reasonForStartingProcess","reason for processing"},
                                    {"subject","subject"},
                                    {"transferred","transferred"},
                                    {"updatedByEstateOfficer","updated by"},
                                    {"addressStreet1","Address Street 1"},
                                    {"addressStreet2","Address Street 2"},
                                    {"addressStreet3","Address Street 3"},
                                    {"addressZIPPostalCode","E18"},
                                    {"homeCheck","home check"}
                                }
                            }
                        }
                   }
               ), json);
        }

        [Fact]
        public void GetInterActionsAndChildInteractionsByDateRangeReturnsCorrectResponseWithStatus()
        {
            mockListInteractionsAndChildInteractionsUseCase.Setup(_ => _.Execute(It.IsAny<ListInteractionsAndChildInteractionsRequest>()))
                .Returns(new ListInteractionsAndChildInteractionsResponse(
                    listInteractionsAndChildInteractionsRequest,
                    generatedAt,
                    interactionsAndChildInteraction));

            var response = interactionsController.GetInteractionsAndChildInteractionsByDateRange(listInteractionsAndChildInteractionsRequest);

            var json = JsonConvert.SerializeObject(response.Value);

            Assert.Equal(200, response.StatusCode);

            Assert.Equal(
               JsonConvert.SerializeObject(
                   new Dictionary<string, object>
                   {
                        {
                            "request",
                            new Dictionary<string, object>
                              {
                                { "fromDate", fromDate },
                                { "toDate", toDate }
                              }
                        },
                        { "generatedAt", generatedAt },
                        { "interactions",
                            new [] {
                                new Dictionary<string, object>
                                {
                                    {"childInteractions", new [] {
                                        new Dictionary<string, object>
                                            {
                                                {"id","02d0ff4d-2555-e911-a555-002248072xyz"},
                                                {"parentInteractionId", "a4b44f8e-e714-ea11-a811-000d3a86d68d" },
                                                {"name","CASE-NAME"},
                                                {"createdOn","24/12/2019"},
                                                {"contact","Contact name"},
                                                {"natureofEnquiry","nature of enquiry"},
                                                {"createdByEstateOfficer","Created by estate officer"},
                                                {"enquirySubject","enquiry subject"},
                                                {"estateOfficerPatch","estate officer patch"},
                                                {"handledBy","Handled Officer"},
                                                {"householdInteraction","555"},
                                                {"incident","incident type"},
                                                {"managerPatch","manager patch"},
                                                {"nhoAreaName","NHO area name"},
                                                {"processStage","555"},
                                                {"processType","555"},
                                                {"reasonForStartingProcess","reason for processing"},
                                                {"subject","subject"},
                                                {"transferred","transferred"},
                                                {"updatedByEstateOfficer","updated by"},
                                                {"addressStreet1","Address Street 1"},
                                                {"addressStreet2","Address Street 2"},
                                                {"addressStreet3","Address Street 3"},
                                                {"addressZIPPostalCode","E18"},
                                                {"homeCheck","home check"}
                                            }
                                        }
                                    },
                                    {"id","02d0ff4d-2555-e911-a555-002248072xyz"},
                                    {"parentInteractionId", null },
                                    {"name","CASE-NAME"},
                                    {"createdOn","24/12/2019"},
                                    {"contact","Contact name"},
                                    {"natureofEnquiry","nature of enquiry"},
                                    {"createdByEstateOfficer","Created by estate officer"},
                                    {"enquirySubject","enquiry subject"},
                                    {"estateOfficerPatch","estate officer patch"},
                                    {"handledBy","Handled Officer"},
                                    {"householdInteraction","555"},
                                    {"incident","incident type"},
                                    {"managerPatch","manager patch"},
                                    {"nhoAreaName","NHO area name"},
                                    {"processStage","555"},
                                    {"processType","555"},
                                    {"reasonForStartingProcess","reason for processing"},
                                    {"subject","subject"},
                                    {"transferred","transferred"},
                                    {"updatedByEstateOfficer","updated by"},
                                    {"addressStreet1","Address Street 1"},
                                    {"addressStreet2","Address Street 2"},
                                    {"addressStreet3","Address Street 3"},
                                    {"addressZIPPostalCode","E18"},
                                    {"homeCheck","home check"}
                                }
                            }
                        }
                   }
               ), json);
        }
    }
}
