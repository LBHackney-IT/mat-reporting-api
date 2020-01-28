using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns>A list of ETRA meetings</returns>
        [HttpGet]
        public  JsonResult GetETRAMeetingsByDateRange([FromQuery] ListETRAMeetingsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ResponseBuilder.ErrorFromList(400, ModelState.Values.SelectMany(x => x?.Errors?.Select(y => y?.ErrorMessage)));
            }

            var meetings = _listETRAMeetings.Execute(request);

            return ResponseBuilder.Ok(meetings);
        }
    }
}
