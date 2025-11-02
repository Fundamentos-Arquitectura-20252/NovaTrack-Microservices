using Personnel.API.Domain.Model.Aggregates;
using SharedKernel.Domain.Model;
using Personnel.API.Domain.Model.ValueObjects;

namespace Personnel.API.Domain.Model.Commands
{
    public record UpdateDriverStatusCommand(
        int DriverId,
        DriverStatus Status
    ) : ICommand<bool>;
}
