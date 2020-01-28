using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Builders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MaTReportingAPI.Controllers.V1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class InteractionsController : BaseController
    {
        private readonly IListInteractions _listIntercations;
        private readonly IListInteractionsAndChildInteractions _listInteractionsAndChildInteractions;
       
        public InteractionsController(IListInteractions listInteractions, IListInteractionsAndChildInteractions listInteractionsAndChildInteractions)
        {
            _listIntercations = listInteractions;
            _listInteractionsAndChildInteractions = listInteractionsAndChildInteractions;
        }
        /// <summary>
        /// Returns tenancy visit interactions, including home check flag
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/V1/Interactions")]
        public JsonResult GetInteractionsByDateRange([FromQuery] ListInteractionsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ResponseBuilder.ErrorFromList(400, ModelState.Values.SelectMany(x => x?.Errors?.Select(y => y?.ErrorMessage)));
            }
                
            var interactions = _listIntercations.Execute(request);

            return ResponseBuilder.Ok(interactions);
        }

        [HttpGet]
        [Route("api/V1/Interactions/GetInteractionsAndChildInteractionsByDateRange")]
        public JsonResult GetInteractionsAndChildInteractionsByDateRange([FromQuery] ListInteractionsAndChildInteractionsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ResponseBuilder.ErrorFromList(400, ModelState.Values.SelectMany(x => x?.Errors?.Select(y => y?.ErrorMessage)));
            }

            var interactionsAndChildInteractions = _listInteractionsAndChildInteractions.Execute(request);

            return ResponseBuilder.Ok(interactionsAndChildInteractions);
        }
    }
}
