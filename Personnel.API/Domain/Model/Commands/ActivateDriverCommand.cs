using SharedKernel.Domain.Model;

namespace Personnel.API.Domain.Model.Commands
{
    public record ActivateDriverCommand(
        int DriverId
    ) : ICommand<bool>;
}
