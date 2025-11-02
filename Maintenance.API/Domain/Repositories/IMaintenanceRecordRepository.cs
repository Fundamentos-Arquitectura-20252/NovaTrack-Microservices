using Maintenance.API.Domain.Model.Aggregates;
using SharedKernel.Domain.Repositories;

namespace Maintenance.API.Domain.Repositories
{
    public interface IMaintenanceRecordRepository : IBaseRepository<MaintenanceRecord>
    {
        Task<IEnumerable<MaintenanceRecord>> FindByVehicleIdAsync(int vehicleId);
        Task<IEnumerable<MaintenanceRecord>> FindByStatusAsync(MaintenanceStatus status);
        Task<IEnumerable<MaintenanceRecord>> FindByTypeAsync(MaintenanceType type);
        Task<IEnumerable<MaintenanceRecord>> FindOverdueAsync();
        Task<IEnumerable<MaintenanceRecord>> FindUpcomingAsync(int daysThreshold = 7);
        Task<IEnumerable<MaintenanceRecord>> FindByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaintenanceRecord>> FindByServiceProviderAsync(string serviceProvider);
        Task<MaintenanceRecord?> FindPendingByVehicleAndTypeAsync(int vehicleId, MaintenanceType type);
        Task<IEnumerable<MaintenanceRecord>> FindScheduledForDateAsync(DateTime date);
        Task<decimal> GetTotalCostByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaintenanceRecord>> FindByPriorityAsync(int priority);
        Task<Dictionary<MaintenanceType, int>> GetMaintenanceCountByTypeAsync();
        Task<Dictionary<MaintenanceStatus, int>> GetMaintenanceCountByStatusAsync();
    }
}