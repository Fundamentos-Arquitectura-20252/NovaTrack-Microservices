namespace Maintenance.API.Interfaces.REST.Resources
{
    public record MaintenanceRecordResource(
        int Id,
        int VehicleId,
        string VehicleLicensePlate,
        string VehicleModel,
        string Description,
        string Type,
        decimal EstimatedCost,
        decimal? ActualCost,
        string Currency,
        DateTime ScheduledDate,
        DateTime? CompletedDate,
        string Status,
        int Priority,
        bool IsOverdue,
        bool IsUpcoming,
        int DaysUntilScheduled,
        string ServiceProvider,
        string Notes,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
