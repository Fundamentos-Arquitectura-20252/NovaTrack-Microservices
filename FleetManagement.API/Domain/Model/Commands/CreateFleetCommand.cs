using SharedKernel.Domain.Model;
using FleetManagement.API.Domain.Model.ValueObjects;

namespace FleetManagement.API.Domain.Model.Commands
{
    public record CreateFleetCommand(
        string Code,
        string Name,
        string Description,
        FleetType Type
    ) : ICommand<int>;
}
