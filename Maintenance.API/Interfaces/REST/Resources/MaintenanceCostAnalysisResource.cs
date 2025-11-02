namespace Maintenance.API.Interfaces.REST.Resources
{
    public record MaintenanceCostAnalysisResource(
        DateTime StartDate,
        DateTime EndDate,
        decimal TotalCost,
        decimal MaintenanceCost,
        decimal ServiceCost,
        decimal AverageCostPerVehicle,
        decimal CostVariance,
        List<MonthlyCostBreakdown> MonthlyBreakdown,
        List<VehicleCostSummary> VehicleCosts,
        List<ServiceProviderCostSummary> ProviderCosts,
        CostTrends Trends
    );

    public record MonthlyCostBreakdown(
        int Year,
        int Month,
        decimal MaintenanceCost,
        decimal ServiceCost,
        decimal TotalCost
    );

    public record VehicleCostSummary(
        int VehicleId,
        string LicensePlate,
        decimal TotalCost,
        int MaintenanceCount,
        int ServiceCount
    );

    public record ServiceProviderCostSummary(
        string ProviderName,
        decimal TotalCost,
        int ServiceCount,
        decimal AverageCost
    );

    public record CostTrends(
        double MonthlyGrowthRate,
        string Trend,
        List<string> CostDrivers
    );
}
