using Microsoft.EntityFrameworkCore;
using Maintenance.API.Domain.Model.Aggregates;
using Maintenance.API.Domain.Repositories;


namespace Maintenance.API.Infrastructure.Persistence.EFC.Repositories
{
    public class MaintenanceRecordRepository : IMaintenanceRecordRepository
    {
        private readonly MaintenanceDbContext _context;

        public MaintenanceRecordRepository(MaintenanceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaintenanceRecord>> ListAsync()
        {
            return await _context.MaintenanceRecords.ToListAsync();
        }

        public async Task<MaintenanceRecord?> FindByIdAsync(int id)
        {
            return await _context.MaintenanceRecords.FindAsync(id);
        }

        public async Task AddAsync(MaintenanceRecord entity)
        {
            await _context.MaintenanceRecords.AddAsync(entity);
        }

        public void Update(MaintenanceRecord entity)
        {
            _context.MaintenanceRecords.Update(entity);
        }

        public void Remove(MaintenanceRecord entity)
        {
            _context.MaintenanceRecords.Remove(entity);
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindByVehicleIdAsync(int vehicleId)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindByStatusAsync(MaintenanceStatus status)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindByTypeAsync(MaintenanceType type)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindOverdueAsync()
        {
            // No tenemos ScheduledDate, así que no podemos definir “overdue”.
            // Aquí podrías definirlo en base a Status, por ejemplo:
            return await _context.MaintenanceRecords
                .Where(m => m.Status == MaintenanceStatus.InProgress)
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindUpcomingAsync(int daysThreshold = 7)
        {
            // Igual, sin ScheduledDate, retornamos vacía o futura extensión
            return Enumerable.Empty<MaintenanceRecord>();
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.CreatedAt >= startDate && m.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindByServiceProviderAsync(string serviceProvider)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.ServiceProvider == serviceProvider)
                .ToListAsync();
        }

        public async Task<MaintenanceRecord?> FindPendingByVehicleAndTypeAsync(int vehicleId, MaintenanceType type)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.VehicleId == vehicleId && m.Type == type && m.Status == MaintenanceStatus.InProgress)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindScheduledForDateAsync(DateTime date)
        {
            // Sin ScheduledDate, se devuelve vacío
            return Enumerable.Empty<MaintenanceRecord>();
        }

        public async Task<decimal> GetTotalCostByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.CreatedAt >= startDate && m.CreatedAt <= endDate)
                .SumAsync(m => m.Cost);
        }

        public async Task<IEnumerable<MaintenanceRecord>> FindByPriorityAsync(int priority)
        {
            // Sin campo Priority, se devuelve vacío
            return Enumerable.Empty<MaintenanceRecord>();
        }

        public async Task<Dictionary<MaintenanceType, int>> GetMaintenanceCountByTypeAsync()
        {
            return await _context.MaintenanceRecords
                .GroupBy(m => m.Type)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<MaintenanceStatus, int>> GetMaintenanceCountByStatusAsync()
        {
            return await _context.MaintenanceRecords
                .GroupBy(m => m.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
}
