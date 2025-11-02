using SharedKernel.Domain.Model;

namespace Personnel.API.Domain.Model.Commands
{
    public record RenewDriverLicenseCommand(
        int DriverId,
        string NewLicenseNumber,
        DateTime NewExpiryDate
    ) : ICommand<bool>;
}
