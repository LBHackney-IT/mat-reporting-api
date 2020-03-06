using MaTReportingAPI.Controllers.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Domain;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

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

        private static readonly Interaction interaction = Helpers.InteractionsHelper.GetInteraction();
        readonly List<Interaction> interactions = new List<Interaction>() { interaction };

        //private List<Interaction> interactionsAndChildInteraction = new List<Interaction>();

        public InteractionsControllerTests()
        {
            mockListInteractionsUseCase = new Mock<IListInteractions>();
            mockListInteractionsAndChildInteractionsUseCase = new Mock<IListInteractionsAndChildInteractions>();

            interactionsController = new InteractionsController(
                mockListInteractionsUseCase.Object,
                mockListInteractionsAndChildInteractionsUseCase.Object);
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
                                Helpers.InteractionsHelper.GetInteractionsAsDictionaryObject()
                            }
                        }
                   }
               ), json);;
        }

        [Fact]
        public void GetInterActionsAndChildInteractionsByDateRangeReturnsCorrectResponseWithStatus()
        {
            mockListInteractionsAndChildInteractionsUseCase.Setup(_ => _.Execute(It.IsAny<ListInteractionsAndChildInteractionsRequest>()))
                .Returns(new ListInteractionsAndChildInteractionsResponse(
                    listInteractionsAndChildInteractionsRequest,
                    generatedAt,
                    interactions));

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
                                Helpers.InteractionsHelper.GetInteractionsAsDictionaryObject()
                            }
                        }
                   }
               ), json);
        }
    }
}
