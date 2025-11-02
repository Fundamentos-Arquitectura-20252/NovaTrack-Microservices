using SharedKernel.Domain.Model;

namespace Personnel.API.Domain.Model.Commands
{
    public record RegisterDriverCommand(
        string Code,
        string FirstName,
        string LastName,
        string LicenseNumber,
        DateTime LicenseExpiryDate,
        string Phone,
        string Email,
        int ExperienceYears
    ) : ICommand<int>;
}
