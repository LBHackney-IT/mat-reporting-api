using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MaTReportingAPI.Controllers.V1;
using MaTReportingAPI.UseCase.V1;
using Xunit;

namespace UnitTests.V1.Controllers
{
    public class HealthCheckControllerTests
    {
        private HealthCheckController _classUnderTest;

        public HealthCheckControllerTests()
        {
            _classUnderTest = new HealthCheckController();
        }

        [Fact]
        public void ReturnsResponseWithStatus()
        {
            var response = _classUnderTest.HealthCheck() as OkObjectResult;

            Assert.NotNull(response);
            Assert.Equal(200, response.StatusCode);
            Assert.Equal(new Dictionary<string, object> {{"success", true}}, response.Value);

        }

        [Fact]
        public void ThrowErrorThrows()
        {
            Assert.Throws<TestOpsErrorException>(_classUnderTest.ThrowError);
        }
    }
}
