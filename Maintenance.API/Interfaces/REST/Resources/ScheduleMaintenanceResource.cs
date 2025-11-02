namespace Maintenance.API.Interfaces.REST.Resources
{
    public record ScheduleMaintenanceResource(
        int VehicleId,
        string Description,
        string Type,
        decimal EstimatedCost,
        DateTime ScheduledDate,
        string ServiceProvider,
        int Priority = 3
    );
}
