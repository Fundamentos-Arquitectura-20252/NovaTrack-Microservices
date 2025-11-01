using SharedKernel.Domain.Model;

namespace FleetManagement.API.Domain.Model.Commands
{
    public record AssignVehicleToDriverCommand(
        int VehicleId,
        int DriverId
    ) : ICommand<bool>;
}
