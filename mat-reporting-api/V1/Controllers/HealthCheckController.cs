using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MaTReportingAPI.UseCase.V1;
using System;

namespace MaTReportingAPI.Controllers.V1
{
    [Route("api/v1/healthcheck")]
    [ApiController]
    [Produces("application/json")]
    public class HealthCheckController : BaseController
    {
        [HttpGet]
        [Route("ping")]
        [ProducesResponseType(typeof(Dictionary<string, bool>), 200)]
        public IActionResult HealthCheck()
        {
            var result = new Dictionary<string, bool> {{"success", true} };

            return Ok(result);
        }

        [HttpGet]
        [Route("error")]
        public void ThrowError()
        {
            ThrowOpsErrorUsecase.Execute();
        }

        [HttpGet]
        [Route("environment")]
        [ProducesResponseType(typeof(Dictionary<string, string>), 200)]
        public IActionResult GetEnvironment()
        {
            var result = new Dictionary<string, string> { { "ENV", Environment.GetEnvironmentVariable("Environment")?.ToString() } };

            return Ok(result);
        }

    }
}
