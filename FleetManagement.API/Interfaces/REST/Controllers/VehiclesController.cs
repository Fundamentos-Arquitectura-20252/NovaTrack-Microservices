using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using FleetManagement.API.Domain.Model.Commands;
using FleetManagement.API.Domain.Model.Queries;
using FleetManagement.API.Interfaces.REST.Resources;
using FleetManagement.API.Interfaces.REST.Transform;

namespace FleetManagement.API.Interfaces.REST.Controllers
{
  
    
    [ApiController] // Indicates that this class is an API controller
    [Route("/api/vehicles")] // Defines the route for the controller
    [Produces(MediaTypeNames.Application.Json)] // Specifies that the API produces JSON responses
    [Tags("Vehicles")] // Adds a tag for grouping in Swagger documentation
    public class VehiclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehiclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieve vehicles",
            Description = "Retrieves all vehicles, with optional filtering by fleet, status, or maintenance state.",
            OperationId = "GetVehicles")]
        [SwaggerResponse(StatusCodes.Status200OK, "Vehicles retrieved successfully.", typeof(IEnumerable<VehicleResource>))]
        public async Task<ActionResult<IEnumerable<VehicleResource>>> GetVehicles([FromQuery] int? fleetId = null, [FromQuery] string? status = null)
        {
            object query = (fleetId, status) switch
            {
                (not null, _) => new GetVehiclesByFleetIdQuery(fleetId.Value),
                (_, "maintenance") => new GetVehiclesInMaintenanceQuery(),
                (_, "service-due") => new GetVehiclesDueForServiceQuery(),
                (_, not null) => new GetVehiclesByStatusQuery(status),
                _ => new GetAllVehiclesQuery()
            };

            var vehicles = await _mediator.Send(query);
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Retrieve vehicle by ID",
            Description = "Gets detailed information for a specific vehicle by its ID.",
            OperationId = "GetVehicleById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Vehicle retrieved successfully.", typeof(VehicleResource))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Vehicle not found.")]
        public async Task<ActionResult<VehicleResource>> GetVehicle(int id)
        {
            var vehicle = await _mediator.Send(new GetVehicleByIdQuery(id));
            return vehicle == null
                ? NotFound(new { success = false, message = "Vehicle not found" })
                : Ok(vehicle);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create new vehicle",
            Description = "Creates a new vehicle record and assigns it to a fleet if specified.",
            OperationId = "CreateVehicle")]
        [SwaggerResponse(StatusCodes.Status201Created, "Vehicle created successfully.", typeof(VehicleResource))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid vehicle data.")]
        public async Task<ActionResult<VehicleResource>> CreateVehicle([FromBody] CreateVehicleResource resource)
        {
            var command = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
            var vehicleId = await _mediator.Send(command);
            var vehicle = await _mediator.Send(new GetVehicleByIdQuery(vehicleId));
            return CreatedAtAction(nameof(GetVehicle), new { id = vehicleId }, vehicle);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update vehicle by ID",
            Description = "Updates vehicle details such as model, license, or status.",
            OperationId = "UpdateVehicleById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Vehicle updated successfully.", typeof(VehicleResource))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Vehicle not found.")]
        public async Task<ActionResult<VehicleResource>> UpdateVehicle(int id, [FromBody] UpdateVehicleResource resource)
        {
            var command = UpdateVehicleCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await _mediator.Send(command);
            if (!result) return NotFound(new { success = false, message = "Vehicle not found" });

            var vehicle = await _mediator.Send(new GetVehicleByIdQuery(id));
            return Ok(vehicle);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete vehicle by ID",
            Description = "Deletes a specific vehicle record from the fleet management system.",
            OperationId = "DeleteVehicleById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Vehicle deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Vehicle not found.")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            var result = await _mediator.Send(new DeleteVehicleCommand(id));
            return result
                ? Ok(new { success = true, message = "Vehicle deleted successfully" })
                : NotFound(new { success = false, message = "Vehicle not found" });
        }

        [HttpPost("{id}/assign-fleet")]
        [SwaggerOperation(
            Summary = "Assign vehicle to fleet",
            Description = "Assigns a specific vehicle to a given fleet.",
            OperationId = "AssignVehicleToFleet")]
        [SwaggerResponse(StatusCodes.Status200OK, "Vehicle assigned successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Vehicle not found.")]
        public async Task<ActionResult> AssignToFleet(int id, [FromBody] AssignVehicleToFleetResource resource)
        {
            var command = AssignVehicleCommandFromResourceAssembler.ToFleetCommandFromResource(id, resource);
            var result = await _mediator.Send(command);
            return result
                ? Ok(new { success = true, message = "Vehicle assigned to fleet successfully" })
                : NotFound(new { success = false, message = "Vehicle not found" });
        }

        [HttpPost("{id}/update-mileage")]
        [SwaggerOperation(
            Summary = "Update vehicle mileage",
            Description = "Updates the mileage reading of a vehicle.",
            OperationId = "UpdateVehicleMileage")]
        [SwaggerResponse(StatusCodes.Status200OK, "Mileage updated successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Vehicle not found.")]
        public async Task<ActionResult> UpdateMileage(int id, [FromBody] UpdateMileageResource resource)
        {
            var command = UpdateMileageCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await _mediator.Send(command);
            return result
                ? Ok(new { success = true, message = "Mileage updated successfully" })
                : NotFound(new { success = false, message = "Vehicle not found" });
        }
    }
}
