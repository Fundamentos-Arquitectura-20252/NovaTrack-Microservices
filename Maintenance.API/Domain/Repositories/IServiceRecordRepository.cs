using Maintenance.API.Domain.Model.Aggregates;
using SharedKernel.Domain.Repositories;

namespace Maintenance.API.Domain.Repositories
{
    public interface IServiceRecordRepository : IBaseRepository<ServiceRecord>
    {
        Task<IEnumerable<ServiceRecord>> FindByVehicleIdAsync(int vehicleId);
        Task<IEnumerable<ServiceRecord>> FindByServiceTypeAsync(ServiceType serviceType);
        Task<IEnumerable<ServiceRecord>> FindByServiceProviderAsync(string serviceProvider);
        Task<IEnumerable<ServiceRecord>> FindByTechnicianAsync(string technicianName);
        Task<IEnumerable<ServiceRecord>> FindByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ServiceRecord>> FindRecentServicesAsync(int daysThreshold = 30);
        Task<IEnumerable<ServiceRecord>> FindByQualityAsync(ServiceQuality quality);
        Task<ServiceRecord?> FindLatestByVehicleIdAsync(int vehicleId);
        Task<ServiceRecord?> FindLatestByVehicleAndTypeAsync(int vehicleId, ServiceType serviceType);
        Task<decimal> GetTotalServiceCostByVehicleAsync(int vehicleId);
        Task<decimal> GetTotalServiceCostByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<ServiceType, int>> GetServiceCountByTypeAsync();
        Task<Dictionary<string, int>> GetServiceCountByProviderAsync();
        Task<Dictionary<ServiceQuality, int>> GetServiceCountByQualityAsync();
        Task<IEnumerable<ServiceRecord>> FindServicesUnderWarrantyAsync();
        Task<double> GetAverageServiceCostAsync();
        Task<double> GetAverageServiceCostByTypeAsync(ServiceType serviceType);
    }
}