using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Analytics.API.Domain.Model.Queries;
using Analytics.API.Interfaces.REST.Resources;

namespace Analytics.API.Interfaces.REST.Controllers
{
    [ApiController]
    [Route("/api")] // Defines the route for the controller
    [Produces(MediaTypeNames.Application.Json)] // Specifies that the API produces JSON responses
    [Tags("Dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get dashboard statistics
        /// </summary>
        [HttpGet("dashboard/stats")]
        public async Task<ActionResult<DashboardStatsResource>> GetDashboardStats()
        {
            try
            {
                var query = new GetDashboardStatsQuery();
                var stats = await _mediator.Send(query);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving dashboard statistics",
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Get fleet summary for dashboard
        /// </summary>
        [HttpGet("dashboard/fleet-summary")]
        public async Task<ActionResult<FleetSummaryResource>> GetFleetSummary()
        {
            try
            {
                var query = new GetFleetSummaryQuery();
                var summary = await _mediator.Send(query);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving fleet summary",
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Get active vehicles for dashboard
        /// </summary>
        [HttpGet("dashboard/active-vehicles")]
        public async Task<ActionResult<IEnumerable<ActiveVehicleResource>>> GetActiveVehicles()
        {
            try
            {
                var query = new GetActiveVehiclesQuery();
                var vehicles = await _mediator.Send(query);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving active vehicles",
                    error = ex.Message 
                });
            }
        }
        [HttpGet("health")]
        public ActionResult HealthCheck()
        {
            return Ok(new {
                success = true,
                message = "Analytics service is running",
                timestamp = DateTime.UtcNow
            });
        }
    }
}