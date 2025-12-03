using System.Net.Mime;
using Maintenance.API.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Maintenance.API.Domain.Model.Commands;
using Maintenance.API.Domain.Model.Queries;
using Maintenance.API.Interfaces.REST.Resources;
using Maintenance.API.Domain.Repositories;

// MaintenanceController.cs
namespace Maintenance.API.Interfaces.REST.Controllers
{
    [ApiController]
    [Route("/api")] // Defines the route for the controller
    [Produces(MediaTypeNames.Application.Json)] // Specifies that the API produces JSON responses
    [Tags("Maintenance")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaintenanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("records")]
        public async Task<ActionResult<IEnumerable<MaintenanceRecordResource>>> GetMaintenanceRecords(
            [FromQuery] int? vehicleId = null,
            [FromQuery] string? status = null,
            [FromQuery] string? type = null,
            [FromQuery] bool overdue = false,
            [FromQuery] bool upcoming = false)
        {
            try
            {
                object query = (vehicleId, status, type, overdue, upcoming) switch
                {
                    (not null, _, _, _, _) => new GetMaintenanceByVehicleIdQuery(vehicleId.Value),
                    (_, not null, _, _, _) when Enum.TryParse<MaintenanceStatus>(status, true, out var statusEnum) => 
                        new GetMaintenanceByStatusQuery(statusEnum),
                    (_, _, not null, _, _) when Enum.TryParse<MaintenanceType>(type, true, out var typeEnum) => 
                        new GetMaintenanceByTypeQuery(typeEnum),
                    (_, _, _, true, _) => new GetOverdueMaintenanceQuery(),
                    (_, _, _, _, true) => new GetUpcomingMaintenanceQuery(),
                    _ => new GetAllMaintenanceRecordsQuery()
                };

                var records = await _mediator.Send(query);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving maintenance records",
                    error = ex.Message 
                });
            }
        }

        [HttpGet("records/{id}")]
        public async Task<ActionResult<MaintenanceRecordResource>> GetMaintenanceRecord(int id)
        {
            try
            {
                var query = new GetMaintenanceRecordByIdQuery(id);
                var record = await _mediator.Send(query);
                
                if (record == null)
                    return NotFound(new { success = false, message = "Maintenance record not found" });
                    
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving maintenance record",
                    error = ex.Message 
                });
            }
        }

        [HttpPost("records")]
        public async Task<ActionResult<MaintenanceRecordResource>> ScheduleMaintenance([FromBody] ScheduleMaintenanceResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!Enum.TryParse<MaintenanceType>(resource.Type, true, out var type))
                    return BadRequest(new { success = false, message = "Invalid maintenance type" });

                var command = new ScheduleMaintenanceCommand(
                    resource.VehicleId,
                    resource.Description,
                    type,
                    resource.EstimatedCost,
                    resource.ScheduledDate,
                    resource.ServiceProvider,
                    resource.Priority
                );

                var maintenanceId = await _mediator.Send(command);
                
                var query = new GetMaintenanceRecordByIdQuery(maintenanceId);
                var maintenance = await _mediator.Send(query);
                
                return CreatedAtAction(nameof(GetMaintenanceRecord), new { id = maintenanceId }, maintenance);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error scheduling maintenance",
                    error = ex.Message 
                });
            }
        }

        [HttpPost("records/{id}/start")]
        public async Task<ActionResult> StartMaintenance(int id)
        {
            try
            {
                var command = new StartMaintenanceCommand(id);
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { success = false, message = "Maintenance record not found" });
                
                return Ok(new { success = true, message = "Maintenance started successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("records/{id}/complete")]
        public async Task<ActionResult> CompleteMaintenance(int id, [FromBody] CompleteMaintenanceResource resource)
        {
            try
            {
                var command = new CompleteMaintenanceCommand(id, resource.ActualCost, resource.CompletionNotes);
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { success = false, message = "Maintenance record not found" });
                
                return Ok(new { success = true, message = "Maintenance completed successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("records/{id}/reschedule")]
        public async Task<ActionResult> RescheduleMaintenance(int id, [FromBody] RescheduleMaintenanceResource resource)
        {
            try
            {
                var command = new RescheduleMaintenanceCommand(id, resource.NewScheduledDate, resource.Reason);
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { success = false, message = "Maintenance record not found" });
                
                return Ok(new { success = true, message = "Maintenance rescheduled successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("services")]
        public async Task<ActionResult<IEnumerable<ServiceRecordResource>>> GetServiceRecords(
            [FromQuery] int? vehicleId = null,
            [FromQuery] string? serviceType = null,
            [FromQuery] string? provider = null)
        {
            try
            {
                object query = (vehicleId, serviceType, provider) switch
                {
                    (not null, _, _) => new GetServiceRecordsByVehicleIdQuery(vehicleId.Value),
                    (_, not null, _) when Enum.TryParse<ServiceType>(serviceType, true, out var typeEnum) => 
                        new GetServiceRecordsByTypeQuery(typeEnum),
                    (_, _, not null) => new GetServiceRecordsByProviderQuery(provider),
                    _ => new GetAllServiceRecordsQuery()
                };

                var records = await _mediator.Send(query);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving service records",
                    error = ex.Message 
                });
            }
        }

        [HttpPost("services")]
        public async Task<ActionResult<ServiceRecordResource>> CreateServiceRecord([FromBody] CreateServiceRecordResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!Enum.TryParse<ServiceType>(resource.ServiceType, true, out var serviceType))
                    return BadRequest(new { success = false, message = "Invalid service type" });

                var command = new CreateServiceRecordCommand(
                    resource.VehicleId,
                    serviceType,
                    resource.Description,
                    resource.Cost,
                    resource.ServiceDate,
                    resource.MileageAtService,
                    resource.ServiceProvider,
                    resource.TechnicianName,
                    resource.PartsUsed,
                    resource.Notes
                );

                var serviceId = await _mediator.Send(command);
                
                var query = new GetServiceRecordByIdQuery(serviceId);
                var service = await _mediator.Send(query);
                
                return CreatedAtAction("GetServiceRecord", new { id = serviceId }, service);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error creating service record",
                    error = ex.Message 
                });
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<MaintenanceStatsResource>> GetMaintenanceStats()
        {
            try
            {
                var query = new GetMaintenanceStatsQuery();
                var stats = await _mediator.Send(query);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving maintenance statistics",
                    error = ex.Message 
                });
            }
        }
        
        [HttpGet("health")]
        public ActionResult HealthCheck()
        {
            return Ok(new {
                success = true,
                message = "Maintenance service is running",
                timestamp = DateTime.UtcNow
            });
        }
    }
}

