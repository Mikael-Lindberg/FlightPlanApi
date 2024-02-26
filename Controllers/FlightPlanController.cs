using FlightPlanApi.Data;
using FlightPlanApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> FlightPlanList()
        {

        }

        public async Task<IActionResult> GetFlightPlanById(string flightPlanId)
        {

        }

        public async Task<IActionResult> FileFlightPlan(FlightPlan flightPlan)
        {

        }

        public async Task<IActionResult> UpdateFlightPlan(FlightPlan flightPlan)
        {

        }

        public async Task<IActionResult> DeleteFlightPlan(string flightPlanId)
        {

        }

        public async Task<IActionResult> GetFlightPlanDepartureAirport(string flightPlanId)
        {

        }

        public async Task<IActionResult> GetFlightPlanRoute(string flightPlanId)
        {

        }

        public async Task<IActionResult> GetFlightPlanTimeEnroute(string flightPlanId)
        {

        }
    }
}
