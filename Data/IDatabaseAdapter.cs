using FlightPlanApi.Models;

namespace FlightPlanApi.Data
{
    public interface IDatabaseAdapter
    {
        Task<List<FlightPlan>> GetAllFightPlans();
        Task<FlightPlan> GetFlightPlanById(string flightPlanId);
        Task<TransactionResult> FileFlightPlan(FlightPlan flightPlan);
        Task<TransactionResult> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan);
        Task<bool> DeleteFlightPlanById(string flightPlanId);
    }
}
