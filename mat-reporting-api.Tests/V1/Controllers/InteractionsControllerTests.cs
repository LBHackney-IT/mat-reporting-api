using MaTReportingAPI.Controllers.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.CustomExceptions;
using MaTReportingAPI.V1.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        private static readonly Interaction interaction = Helpers.InteractionsHelper.GetInteraction();
        readonly List<Interaction> interactions = new List<Interaction>() { interaction };

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
            var result = (ObjectResult)response;
            var json = JsonConvert.SerializeObject(result.Value);

            Assert.Equal(200, result.StatusCode);

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

            var result = (ObjectResult)response;

            var json = JsonConvert.SerializeObject(result.Value);

            Assert.Equal(200, result.StatusCode);

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

        [Fact]
        public void Given_UseCase_throws_an_exception_then_controller_returns_status_code_500()
        {
            //Arrange
            mockListInteractionsAndChildInteractionsUseCase
                .Setup(x => x.Execute(It.IsAny<ListInteractionsAndChildInteractionsRequest>())).Throws<Exception>();

            //Act
            var response = interactionsController.GetInteractionsAndChildInteractionsByDateRange(null);

            var result = (StatusCodeResult)response;

            //Assert
            Assert.Equal(result.StatusCode, StatusCodes.Status500InternalServerError);

        }

        [Fact]
        public void Given_that_MaTProcessApiException_exception_is_thrown_then_controller_returns_status_code_500()
        {
            //Arrange
            mockListInteractionsAndChildInteractionsUseCase.Setup(x => x.Execute(It.IsAny<ListInteractionsAndChildInteractionsRequest>())).Throws<MaTProcessApiException>();

            //Act
            var response = interactionsController.GetInteractionsAndChildInteractionsByDateRange(null);
            var result = (ObjectResult)response;

            //Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void Given_that_CRMTokenException_exception_is_thrown_then_controller_returns_status_code_500()
        {
            //Arrange
            mockListInteractionsAndChildInteractionsUseCase.Setup(x => x.Execute(It.IsAny<ListInteractionsAndChildInteractionsRequest>())).Throws<CRMTokenException>();

            //Act
            var response = interactionsController.GetInteractionsAndChildInteractionsByDateRange(null);
            var result = (ObjectResult)response;

            //Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void Given_that_CRMException_exception_is_thrown_then_controller_returns_status_code_500()
        {
            //Arrange
            mockListInteractionsAndChildInteractionsUseCase.Setup(x => x.Execute(It.IsAny<ListInteractionsAndChildInteractionsRequest>())).Throws<CRMException>();

            //Act
            var response = interactionsController.GetInteractionsAndChildInteractionsByDateRange(null);
            var result = (ObjectResult)response;

            //Assert
            Assert.Equal(500, result.StatusCode);
        }
    }
}
