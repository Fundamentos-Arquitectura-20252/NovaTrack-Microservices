using Personnel.API.Domain.Model.Aggregates;
using SharedKernel.Domain.Model;
using Personnel.API.Domain.Model.ValueObjects;

namespace Personnel.API.Domain.Model.Commands
{
    public record UpdateDriverCommand(
        int DriverId,
        string FirstName,
        string LastName,
        string LicenseNumber,
        DateTime LicenseExpiryDate,
        string Phone,
        string Email,
        int ExperienceYears,
        DriverStatus Status
    ) : ICommand<bool>;
}
