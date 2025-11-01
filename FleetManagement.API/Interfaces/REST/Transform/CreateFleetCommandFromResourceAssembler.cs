using FleetManagement.API.Domain.Model.Commands;
using FleetManagement.API.Interfaces.REST.Resources;

namespace FleetManagement.API.Interfaces.REST.Transform
{
    public static class CreateFleetCommandFromResourceAssembler
    {
        public static CreateFleetCommand ToCommandFromResource(CreateFleetResource resource)
        {
            if (!Enum.TryParse<Domain.Model.ValueObjects.FleetType>(resource.Type, true, out var fleetType))
                throw new ArgumentException($"Invalid fleet type: {resource.Type}");

            return new CreateFleetCommand(
                resource.Code,
                resource.Name,
                resource.Description,
                fleetType
            );
        }
    }
}
