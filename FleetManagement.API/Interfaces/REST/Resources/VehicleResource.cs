namespace FleetManagement.API.Interfaces.REST.Resources
{
    public record VehicleResource(
        int Id,
        string LicensePlate,
        string Brand,
        string Model,
        int Year,
        int Mileage,
        string Status,
        int? FleetId,
        string? FleetName,
        int? DriverId,
        string? DriverName,
        DateTime? LastServiceDate,
        DateTime? NextServiceDate,
        bool IsServiceDue,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
