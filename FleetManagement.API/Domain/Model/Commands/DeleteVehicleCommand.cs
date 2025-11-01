using SharedKernel.Domain.Model;

namespace FleetManagement.API.Domain.Model.Commands
{
    public record DeleteVehicleCommand(
        int VehicleId
    ) : ICommand<bool>;
}
