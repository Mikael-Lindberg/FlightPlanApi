using FlightPlanApi.Data;
using FlightPlanApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FlightPlanApi.Controllers
{
    [Route("api/v1/flightplan")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private IDatabaseAdapter _database;

        public FlightPlanController(IDatabaseAdapter database)
        {
            _database = database;
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "No flight plans have been filed with this system.")]
        public async Task<IActionResult> FlightPlanList()
        {
            var flightPlanList = await _database.GetAllFightPlans();

            if (flightPlanList.Count == 0)
            {
                return NoContent();
            }

            return Ok(flightPlanList);
        }

        [HttpGet]
        [Authorize]
        [Route("{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanById(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);

            if (flightPlan.FlightPlanId != flightPlanId)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(flightPlan);
        }

        /// <summary>
        /// Files a new flight plan with the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/flightplan/file
        ///     {
        ///         "dwadaw": "dwad" ... 
        ///     }
        /// </remarks>
        /// <param name="flightPlan">The flight plan data to be filed.</param>
        /// <response code="400">There is a problem with the flight plan data received by this system.</response>
        /// <response code="500">The flight plan is valid but this sytem cannot process it.</response>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("file")]
        public async Task<IActionResult> FileFlightPlan(FlightPlan flightPlan)
        {
            var transactionResult = await _database.FileFlightPlan(flightPlan);

            switch(transactionResult)
            {
                case TransactionResult.Success:
                    return Ok(flightPlan);
                case TransactionResult.BadRequest:
                    return StatusCode(StatusCodes.Status400BadRequest);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateFlightPlan(FlightPlan flightPlan)
        {
            var updateResult = await _database.UpdateFlightPlan(flightPlan.FlightPlanId, flightPlan);

            switch (updateResult)
            {
                case TransactionResult.Success:
                    return Ok();
                case TransactionResult.NotFound:
                    return StatusCode(StatusCodes.Status404NotFound);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{flightPlanId}")]
        public async Task<IActionResult> DeleteFlightPlan(string flightPlanId)
        {
            var result = await _database.DeleteFlightPlanById(flightPlanId);

            if (result) return Ok();

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpGet]
        [Authorize]
        [Route("airport/departure/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanDepartureAirport(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);

            if (flightPlan == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(flightPlan.DepartureAirport);
        }

        [HttpGet]
        [Authorize]
        [Route("route/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanRoute(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);

            if (flightPlan == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(flightPlan.Route);
        }

        [HttpGet]
        [Authorize]
        [Route("time/enroute/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanTimeEnroute(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);

            if (flightPlan == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var estimatedTimeEnroute = flightPlan.ArrivalTime - flightPlan.DepartureTime;

            return Ok(estimatedTimeEnroute);
        }
    }
}
