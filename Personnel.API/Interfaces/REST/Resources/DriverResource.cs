namespace Personnel.API.Interfaces.REST.Resources
{
    public record DriverResource(
        int Id,
        string Code,
        string FirstName,
        string LastName,
        string FullName,
        string LicenseNumber,
        DateTime LicenseExpiryDate,
        bool IsLicenseExpired,
        bool IsLicenseExpiringSoon,
        int DaysUntilLicenseExpiry,
        string Phone,
        string Email,
        int ExperienceYears,
        string ExperienceLevel,
        string Status,
        string? AssignedVehicle,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
