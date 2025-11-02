namespace Maintenance.API.Interfaces.REST.Resources
{
    public record UpdateMaintenanceResource(
        string Description,
        string Type,
        decimal EstimatedCost,
        DateTime ScheduledDate,
        string ServiceProvider,
        string Notes,
        string Status
    );
}
