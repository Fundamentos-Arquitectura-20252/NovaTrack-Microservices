using SharedKernel.Domain.Model;

namespace Personnel.API.Domain.Model.Commands
{
    public record SuspendDriverCommand(
        int DriverId,
        string Reason
    ) : ICommand<bool>;
}
