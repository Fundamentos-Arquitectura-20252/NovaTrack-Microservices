namespace Analytics.API.Application.External.DTO;

public record VehicleDto(
    int Id,
    string LicensePlate,
    string Brand,
    string Model,
    int Year,
    string? DriverName,
    string Status,
    string? FleetName,
    DateTime LastUpdate
);