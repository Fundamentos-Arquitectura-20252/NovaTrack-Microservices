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
    [ApiController]
    [Route("/api/fleets")] // Defines the route for the controller
    [Produces(MediaTypeNames.Application.Json)] // Specifies that the API produces JSON responses
    [Tags("Fleets")]
    public class FleetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FleetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieve fleets",
            Description = "Retrieves all fleets, optionally filtered by active status or type.",
            OperationId = "GetFleets")]
        [SwaggerResponse(StatusCodes.Status200OK, "Fleets retrieved successfully.", typeof(IEnumerable<FleetResource>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while retrieving fleets.")]
        public async Task<ActionResult<IEnumerable<FleetResource>>> GetFleets([FromQuery] bool activeOnly = false, [FromQuery] string? type = null)
        {
            object query = (activeOnly, type) switch
            {
                (true, _) => new GetActiveFleetQuery(),
                (_, not null) => new GetFleetByTypeQuery(type),
                _ => new GetAllFleetsQuery()
            };

            var fleets = await _mediator.Send(query);
            return Ok(fleets);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Retrieve fleet by ID",
            Description = "Gets detailed information for a specific fleet by its unique identifier.",
            OperationId = "GetFleetById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Fleet retrieved successfully.", typeof(FleetResource))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Fleet not found.")]
        public async Task<ActionResult<FleetResource>> GetFleet(int id)
        {
            var fleet = await _mediator.Send(new GetFleetByIdQuery(id));
            return fleet == null ? NotFound(new { success = false, message = "Fleet not found" }) : Ok(fleet);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create new fleet",
            Description = "Creates a new fleet record with the provided details.",
            OperationId = "CreateFleet")]
        [SwaggerResponse(StatusCodes.Status201Created, "Fleet created successfully.", typeof(FleetResource))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid fleet data.")]
        public async Task<ActionResult<FleetResource>> CreateFleet([FromBody] CreateFleetResource resource)
        {
            var command = CreateFleetCommandFromResourceAssembler.ToCommandFromResource(resource);
            var fleetId = await _mediator.Send(command);
            var fleet = await _mediator.Send(new GetFleetByIdQuery(fleetId));

            return CreatedAtAction(nameof(GetFleet), new { id = fleetId }, fleet);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update existing fleet",
            Description = "Updates a fleet’s information by its ID.",
            OperationId = "UpdateFleetById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Fleet updated successfully.", typeof(FleetResource))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Fleet not found.")]
        public async Task<ActionResult<FleetResource>> UpdateFleet(int id, [FromBody] UpdateFleetResource resource)
        {
            var command = UpdateFleetCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await _mediator.Send(command);
            if (!result) return NotFound(new { success = false, message = "Fleet not found" });

            var fleet = await _mediator.Send(new GetFleetByIdQuery(id));
            return Ok(fleet);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete fleet by ID",
            Description = "Deletes a fleet record from the system using its ID.",
            OperationId = "DeleteFleetById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Fleet deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Fleet not found.")]
        public async Task<ActionResult> DeleteFleet(int id)
        {
            var result = await _mediator.Send(new DeleteFleetCommand(id));
            return result
                ? Ok(new { success = true, message = "Fleet deleted successfully" })
                : NotFound(new { success = false, message = "Fleet not found" });
        }
    }
}
