using System.Net.Mime;
using Personnel.API.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Personnel.API.Domain.Model.Commands;
using Personnel.API.Domain.Model.Queries;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Interfaces.REST.Transform;
using Personnel.API.Domain.Model.ValueObjects;

// DriversController.cs
namespace Personnel.API.Interfaces.REST.Controllers
{
    [ApiController]
    [Route("/api/drivers")] // Defines the route for the controller
    [Produces(MediaTypeNames.Application.Json)] // Specifies that the API produces JSON responses
    [Tags("Drivers")]
    public class DriversController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DriversController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverResource>>> GetDrivers([FromQuery] bool activeOnly = false, [FromQuery] string? status = null)
        {
            try
            {
                object query = (activeOnly, status) switch
                {
                    (true, _) => new GetActiveDriversQuery(),
                    (_, "available") => new GetAvailableDriversQuery(),
                    (_, not null) when Enum.TryParse<DriverStatus>(status, true, out var statusEnum) => 
                        new GetDriversByStatusQuery(statusEnum),
                    _ => new GetAllDriversQuery()
                };

                var drivers = await _mediator.Send(query);
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving drivers",
                    error = ex.Message 
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverResource>> GetDriver(int id)
        {
            try
            {
                var query = new GetDriverByIdQuery(id);
                var driver = await _mediator.Send(query);
                
                if (driver == null)
                    return NotFound(new { success = false, message = "Driver not found" });
                    
                return Ok(driver);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving driver",
                    error = ex.Message 
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<DriverResource>> RegisterDriver([FromBody] RegisterDriverResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = RegisterDriverCommandFromResourceAssembler.ToCommandFromResource(resource);
                var driverId = await _mediator.Send(command);
                
                var query = new GetDriverByIdQuery(driverId);
                var driver = await _mediator.Send(query);
                
                return CreatedAtAction(nameof(GetDriver), new { id = driverId }, driver);
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
                    message = "Error registering driver",
                    error = ex.Message 
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DriverResource>> UpdateDriver(int id, [FromBody] UpdateDriverResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = UpdateDriverCommandFromResourceAssembler.ToCommandFromResource(id, resource);
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { success = false, message = "Driver not found" });
                
                var query = new GetDriverByIdQuery(id);
                var driver = await _mediator.Send(query);
                
                return Ok(driver);
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
                    message = "Error updating driver",
                    error = ex.Message 
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeactivateDriver(int id)
        {
            try
            {
                var command = new DeactivateDriverCommand(id);
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { success = false, message = "Driver not found" });
                
                return Ok(new {
                    success = true,
                    message = "Driver deactivated successfully"
                });
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
                    message = "Error deactivating driver",
                    error = ex.Message 
                });
            }
        }

        [HttpPost("{id}/status")]
        public async Task<ActionResult> UpdateDriverStatus(int id, [FromBody] UpdateDriverStatusResource resource)
        {
            try
            {
                var command = UpdateDriverStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { success = false, message = "Driver not found" });
                
                return Ok(new { success = true, message = "Driver status updated successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("{id}/renew-license")]
        public async Task<ActionResult> RenewLicense(int id, [FromBody] RenewLicenseResource resource)
        {
            try
            {
                var command = RenewLicenseCommandFromResourceAssembler.ToCommandFromResource(id, resource);
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { success = false, message = "Driver not found" });
                
                return Ok(new { success = true, message = "License renewed successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DriverStatsResource>> GetDriverStats()
        {
            try
            {
                var query = new GetDriverStatsQuery();
                var stats = await _mediator.Send(query);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving driver statistics",
                    error = ex.Message 
                });
            }
        }

        [HttpGet("licenses/expiring")]
        public async Task<ActionResult<IEnumerable<DriverResource>>> GetDriversWithExpiringSoonLicenses([FromQuery] int days = 30)
        {
            try
            {
                var query = new GetDriversWithExpiringSoonLicensesQuery(days);
                var drivers = await _mediator.Send(query);
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error retrieving drivers with expiring licenses",
                    error = ex.Message 
                });
            }
        }
        [HttpGet("health")]
        public ActionResult HealthCheck()
        {
            return Ok(new {
                success = true,
                message = "Personnel service is running",
                timestamp = DateTime.UtcNow
            });
        }
    }
}

// Transform Assemblers
namespace Personnel.API.Interfaces.REST.Transform
{
    public static class DriverResourceFromEntityAssembler
    {
        public static DriverResource ToResourceFromEntity(Domain.Model.Aggregates.Driver entity)
        {
            var experienceLevel = new ExperienceLevel(entity.ExperienceYears);
            
            return new DriverResource(
                entity.Id,
                entity.Code,
                entity.FirstName,
                entity.LastName,
                entity.FullName,
                entity.License.Number,
                entity.License.ExpiryDate,
                entity.License.IsExpired(),
                entity.License.IsExpiringSoon(),
                entity.License.DaysUntilExpiry(),
                entity.ContactInfo.Phone,
                entity.ContactInfo.Email,
                entity.ExperienceYears,
                experienceLevel.Level,
                entity.Status.ToString(),
                null, // AssignedVehicle - se llenará desde FleetManagement context
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            );
        }
    }

    public static class RegisterDriverCommandFromResourceAssembler
    {
        public static RegisterDriverCommand ToCommandFromResource(RegisterDriverResource resource)
        {
            return new RegisterDriverCommand(
                resource.Code,
                resource.FirstName,
                resource.LastName,
                resource.LicenseNumber,
                resource.LicenseExpiryDate,
                resource.Phone,
                resource.Email,
                resource.ExperienceYears
            );
        }
    }

    public static class UpdateDriverCommandFromResourceAssembler
    {
        public static UpdateDriverCommand ToCommandFromResource(int driverId, UpdateDriverResource resource)
        {
            if (!Enum.TryParse<DriverStatus>(resource.Status, true, out var status))
                throw new ArgumentException($"Invalid driver status: {resource.Status}");

            return new UpdateDriverCommand(
                driverId,
                resource.FirstName,
                resource.LastName,
                resource.LicenseNumber,
                resource.LicenseExpiryDate,
                resource.Phone,
                resource.Email,
                resource.ExperienceYears,
                status
            );
        }
    }

    public static class UpdateDriverStatusCommandFromResourceAssembler
    {
        public static UpdateDriverStatusCommand ToCommandFromResource(int driverId, UpdateDriverStatusResource resource)
        {
            if (!Enum.TryParse<DriverStatus>(resource.Status, true, out var status))
                throw new ArgumentException($"Invalid driver status: {resource.Status}");

            return new UpdateDriverStatusCommand(driverId, status);
        }
    }

    public static class RenewLicenseCommandFromResourceAssembler
    {
        public static RenewDriverLicenseCommand ToCommandFromResource(int driverId, RenewLicenseResource resource)
        {
            return new RenewDriverLicenseCommand(
                driverId,
                resource.NewLicenseNumber,
                resource.NewExpiryDate
            );
        }
    }
    
}