using FleetManagement.API.Domain.Model.Commands;
using FleetManagement.API.Interfaces.REST.Resources;

namespace FleetManagement.API.Interfaces.REST.Transform
{
    public static class CreateVehicleCommandFromResourceAssembler
    {
        public static CreateVehicleCommand ToCommandFromResource(CreateVehicleResource resource)
        {
            return new CreateVehicleCommand(
                resource.LicensePlate,
                resource.Brand,
                resource.Model,
                resource.Year,
                resource.Mileage,
                resource.FleetId,
                resource.DriverId
            );
        }
    }
}
