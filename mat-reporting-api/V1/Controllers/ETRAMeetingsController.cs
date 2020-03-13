using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MaTReportingAPI.Controllers.V1
{
    [Route("api/V1/ETRAMeetings")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class ETRAMeetingsController : BaseController
    {
        private readonly IListETRAMeetings _listETRAMeetings;

        public ETRAMeetingsController(IListETRAMeetings listETRAMeetings)
        {
            _listETRAMeetings = listETRAMeetings;
        }

        /// <summary>
        /// Returns a list ETRA meetings and number of actions taken
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of ETRA meetings</returns>
        [HttpGet]
        public  IActionResult GetETRAMeetingsByDateRange([FromQuery] ListETRAMeetingsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.SelectMany(x => x?.Errors?.Select(y => y?.ErrorMessage)));
                }

                var meetings = _listETRAMeetings.Execute(request);

                return Ok(meetings);
            }
            catch (Exception ex) when (ex is MaTReportingApiBaseException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.GetType().Name.ToString()} : {ex.Message}");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
