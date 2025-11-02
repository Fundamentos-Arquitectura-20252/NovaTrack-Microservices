namespace Analytics.API.Interfaces.REST.Resources
{
    public record ActiveVehicleResource(
        int Id,
        string LicensePlate,
        string Model,
        string DriverName,
        string Status,
        string FleetName,
        DateTime LastUpdate
    );
}