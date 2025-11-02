namespace Maintenance.API.Interfaces.REST.Resources
{
    public record VehicleMaintenanceHistoryResource(
        int VehicleId,
        string LicensePlate,
        string VehicleModel,
        int TotalMaintenanceRecords,
        int TotalServiceRecords,
        decimal TotalMaintenanceCost,
        decimal TotalServiceCost,
        DateTime? LastMaintenanceDate,
        DateTime? LastServiceDate,
        DateTime? NextScheduledMaintenance,
        List<MaintenanceRecordResource> MaintenanceHistory,
        List<ServiceRecordResource> ServiceHistory,
        MaintenanceRecommendations Recommendations
    );

    public record MaintenanceRecommendations(
        List<string> OverdueItems,
        List<string> UpcomingItems,
        List<string> RecommendedServices,
        double MaintenanceScore
    );
}
