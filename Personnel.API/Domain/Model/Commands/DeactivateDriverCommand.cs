using SharedKernel.Domain.Model;

namespace Personnel.API.Domain.Model.Commands
{
    public record DeactivateDriverCommand(
        int DriverId
    ) : ICommand<bool>;
}
