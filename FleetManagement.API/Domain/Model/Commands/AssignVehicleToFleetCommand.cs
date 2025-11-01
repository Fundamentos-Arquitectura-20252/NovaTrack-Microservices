using SharedKernel.Domain.Model;

namespace FleetManagement.API.Domain.Model.Commands
{
    public record AssignVehicleToFleetCommand(
        int VehicleId,
        int FleetId
    ) : ICommand<bool>;
}
