namespace Personnel.API.Interfaces.REST.Resources
{
    public record DriverStatsResource(
        int TotalDrivers,
        int ActiveDrivers,
        int InactiveDrivers,
        int AvailableDrivers,
        int DriversOnRoute,
        int DriversOnBreak,
        int SuspendedDrivers,
        int ExpiredLicenses,
        int LicensesExpiringSoon,
        double AverageExperience,
        int ExperiencedDrivers,
        int SeniorDrivers,
        Dictionary<string, int> DriversByExperienceLevel,
        Dictionary<string, int> DriversByStatus
    );
}
