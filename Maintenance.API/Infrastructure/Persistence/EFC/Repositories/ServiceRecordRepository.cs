using Microsoft.EntityFrameworkCore;
using Maintenance.API.Domain.Model.Aggregates;
using Maintenance.API.Domain.Repositories;

namespace Maintenance.API.Infrastructure.Persistence.EFC.Repositories
{
    public class ServiceRecordRepository : IServiceRecordRepository
    {
        private readonly MaintenanceDbContext _context;

        public ServiceRecordRepository(MaintenanceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceRecord>> ListAsync()
        {
            return await _context.ServiceRecords.ToListAsync();
        }

        public async Task<ServiceRecord?> FindByIdAsync(int id)
        {
            return await _context.ServiceRecords.FindAsync(id);
        }

        public async Task AddAsync(ServiceRecord entity)
        {
            await _context.ServiceRecords.AddAsync(entity);
        }

        public void Update(ServiceRecord entity)
        {
            _context.ServiceRecords.Update(entity);
        }

        public void Remove(ServiceRecord entity)
        {
            _context.ServiceRecords.Remove(entity);
        }

        public async Task<IEnumerable<ServiceRecord>> FindByVehicleIdAsync(int vehicleId)
        {
            return await _context.ServiceRecords
                .Where(s => s.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRecord>> FindByServiceTypeAsync(ServiceType serviceType)
        {
            return await _context.ServiceRecords
                .Where(s => s.ServiceType == serviceType)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRecord>> FindByServiceProviderAsync(string serviceProvider)
        {
            return await _context.ServiceRecords
                .Where(s => s.ServiceProvider == serviceProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRecord>> FindByTechnicianAsync(string technicianName)
        {
            return await _context.ServiceRecords
                .Where(s => s.TechnicianName == technicianName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRecord>> FindByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ServiceRecords
                .Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRecord>> FindRecentServicesAsync(int daysThreshold = 30)
        {
            var fromDate = DateTime.UtcNow.AddDays(-daysThreshold);
            return await _context.ServiceRecords
                .Where(s => s.CreatedAt >= fromDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRecord>> FindByQualityAsync(ServiceQuality quality)
        {
            return await _context.ServiceRecords
                .Where(s => s.Quality == quality)
                .ToListAsync();
        }

        public async Task<ServiceRecord?> FindLatestByVehicleIdAsync(int vehicleId)
        {
            return await _context.ServiceRecords
                .Where(s => s.VehicleId == vehicleId)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceRecord?> FindLatestByVehicleAndTypeAsync(int vehicleId, ServiceType serviceType)
        {
            return await _context.ServiceRecords
                .Where(s => s.VehicleId == vehicleId && s.ServiceType == serviceType)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetTotalServiceCostByVehicleAsync(int vehicleId)
        {
            return await _context.ServiceRecords
                .Where(s => s.VehicleId == vehicleId)
                .SumAsync(s => s.Cost);
        }

        public async Task<decimal> GetTotalServiceCostByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ServiceRecords
                .Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate)
                .SumAsync(s => s.Cost);
        }

        public async Task<Dictionary<ServiceType, int>> GetServiceCountByTypeAsync()
        {
            return await _context.ServiceRecords
                .GroupBy(s => s.ServiceType)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetServiceCountByProviderAsync()
        {
            return await _context.ServiceRecords
                .GroupBy(s => s.ServiceProvider)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<ServiceQuality, int>> GetServiceCountByQualityAsync()
        {
            return await _context.ServiceRecords
                .GroupBy(s => s.Quality)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<IEnumerable<ServiceRecord>> FindServicesUnderWarrantyAsync()
        {
            return await _context.ServiceRecords
                .Where(s => s.IsWarrantyValid(6))
                .ToListAsync();
        }

        public async Task<double> GetAverageServiceCostAsync()
        {
            throw new NotImplementedException();
            // return await _context.ServiceRecords.AverageAsync(s => (double)s.Cost);
        }

        public async Task<double> GetAverageServiceCostByTypeAsync(ServiceType serviceType)
        {
            throw new NotImplementedException();
            /*return await _context.ServiceRecords
                .Where(s => s.ServiceType == serviceType)
                .AverageAsync(s => (double)s.Cost);*/
        }
    }
}
