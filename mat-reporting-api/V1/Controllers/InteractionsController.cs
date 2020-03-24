using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/V1/Interactions")]
        public IActionResult GetInteractionsByDateRange([FromQuery] ListInteractionsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.SelectMany(x => x?.Errors?.Select(y => y?.ErrorMessage)));
                }

                var interactions = _listIntercations.Execute(request);

                return Ok(interactions);
            }
            //thrown when any of the gateways throws custom exception, bubbles up all the way here
            catch(Exception ex) when (ex is MaTReportingApiBaseException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.GetType().Name.ToString()} : {ex.Message}");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/V1/Interactions/GetInteractionsAndChildInteractionsByDateRange")]
        public IActionResult GetInteractionsAndChildInteractionsByDateRange([FromQuery] ListInteractionsAndChildInteractionsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.SelectMany(x => x?.Errors?.Select(y => y?.ErrorMessage)));
                }

                var interactionsAndChildInteractions = _listInteractionsAndChildInteractions.Execute(request);

                return Ok(interactionsAndChildInteractions);
            }
            //thrown when any of the gateways throws custom exception, bubbles up all the way here
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
