using SharedKernel.Domain.Model;

namespace FleetManagement.API.Domain.Model.Commands
{
    public record UpdateVehicleMileageCommand(
        int VehicleId,
        int NewMileage
    ) : ICommand<bool>;
}
