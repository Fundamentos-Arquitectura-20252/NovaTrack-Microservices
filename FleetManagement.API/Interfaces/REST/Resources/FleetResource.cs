namespace FleetManagement.API.Interfaces.REST.Resources
{
    public record FleetResource(
        int Id,
        string Code,
        string Name,
        string Description,
        string Type,
        bool IsActive,
        int VehicleCount,
        int ActiveVehicles,
        int InMaintenanceVehicles,
        double Performance,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
