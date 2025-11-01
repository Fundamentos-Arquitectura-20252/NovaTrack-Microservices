using SharedKernel.Domain.Model;
using FleetManagement.API.Domain.Model.ValueObjects;

namespace FleetManagement.API.Domain.Model.Commands
{
    public record UpdateFleetCommand(
        int FleetId,
        string Name,
        string Description,
        FleetType Type,
        bool IsActive
    ) : ICommand<bool>;
}
