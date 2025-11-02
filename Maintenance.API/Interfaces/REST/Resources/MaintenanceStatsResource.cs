namespace Maintenance.API.Interfaces.REST.Resources
{
    public record MaintenanceStatsResource(
        int TotalMaintenanceRecords,
        int ScheduledMaintenance,
        int InProgressMaintenance,
        int CompletedMaintenance,
        int CancelledMaintenance,
        int OverdueMaintenance,
        int UpcomingMaintenance,
        decimal TotalMaintenanceCost,
        decimal AverageMaintenanceCost,
        decimal MonthlyMaintenanceCost,
        int TotalServiceRecords,
        decimal TotalServiceCost,
        decimal AverageServiceCost,
        Dictionary<string, int> MaintenanceByType,
        Dictionary<string, decimal> CostByType,
        Dictionary<string, int> ServicesByType,
        List<TopServiceProvider> TopServiceProviders
    );

    public record TopServiceProvider(
        string Name,
        int ServiceCount,
        decimal TotalCost,
        double AverageRating
    );
}
