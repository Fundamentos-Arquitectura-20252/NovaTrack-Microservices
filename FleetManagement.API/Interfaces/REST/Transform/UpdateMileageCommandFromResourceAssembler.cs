using FleetManagement.API.Domain.Model.Commands;
using FleetManagement.API.Interfaces.REST.Resources;

namespace FleetManagement.API.Interfaces.REST.Transform
{
    public static class UpdateMileageCommandFromResourceAssembler
    {
        public static UpdateVehicleMileageCommand ToCommandFromResource(int vehicleId, UpdateMileageResource resource)
        {
            return new UpdateVehicleMileageCommand(vehicleId, resource.NewMileage);
        }
    }
}
