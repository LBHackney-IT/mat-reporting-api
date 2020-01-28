using MaTReportingAPI.V1.Builders;
using System.Collections.Generic;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Builders
{
    public class ResponseBuilderTests
    {
        [Fact]
        public void OkReturnsCorrectReponse()
        {
            string responseContent = "Ok method";

            int statusCode = 200;
            string contentType = "application/json";

            var result = ResponseBuilder.Ok(responseContent);

            Assert.Equal(contentType, result.ContentType);
            Assert.Equal(statusCode, result.StatusCode);
            Assert.Equal(responseContent, result.Value);
        }

        [Fact]
        public void ErrorReturnsCorrectResponse()
        {
            string responseContent = "Error method";

            int statusCode = 422;
            string contentType = "application/json";

            var result = ResponseBuilder.Error(statusCode, responseContent);

            Assert.Equal(contentType, result.ContentType);
            Assert.Equal(statusCode, result.StatusCode);
            Assert.Equal(responseContent, result.Value);
        }

        [Fact]
        public void ErrorFromListReturnsCorrectResponse()
        {
            List<string> errorMessages = new List<string>() { "mesage 1", "message 2", "message 3" };

            int statusCode = 400;
            string contentType = "application/json";

            var result = ResponseBuilder.ErrorFromList(statusCode, errorMessages);

            Assert.Equal(contentType, result.ContentType);
            Assert.Equal(statusCode, result.StatusCode);
            Assert.Equal(errorMessages, result.Value);
        }
    }
}
