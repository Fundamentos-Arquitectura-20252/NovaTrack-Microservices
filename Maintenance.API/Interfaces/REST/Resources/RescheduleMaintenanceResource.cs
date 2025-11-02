namespace Maintenance.API.Interfaces.REST.Resources
{
    public record RescheduleMaintenanceResource(
        DateTime NewScheduledDate,
        string Reason
    );
}
