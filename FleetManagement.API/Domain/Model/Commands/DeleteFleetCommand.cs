using SharedKernel.Domain.Model;

namespace FleetManagement.API.Domain.Model.Commands
{
    public record DeleteFleetCommand(
        int FleetId
    ) : ICommand<bool>;
}
