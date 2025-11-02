using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Personnel.API.Domain.Model.Aggregates;
using Personnel.API.Domain.Model.ValueObjects;
using Personnel.API.Domain.Repositories;
using Personnel.API.Infrastructure.Persistence.EFC.Repositories;

// DriverRepository.cs
namespace Personnel.API.Infrastructure.Persistence.EFC.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly PersonnelDbContext _context;

        public DriverRepository(PersonnelDbContext context)
        {
            _context = context;
        }

        public async Task<Driver?> FindByIdAsync(int id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public async Task<IEnumerable<Driver>> ListAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task AddAsync(Driver entity)
        {
            await _context.Drivers.AddAsync(entity);
        }

        public void Update(Driver entity)
        {
            _context.Drivers.Update(entity);
        }

        public void Remove(Driver entity)
        {
            _context.Drivers.Remove(entity);
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _context.Drivers.AnyAsync(d => d.Code == code);
        }

        public async Task<bool> ExistsByLicenseNumberAsync(string licenseNumber)
        {
            return await _context.Drivers.AnyAsync(d => d.License.Number == licenseNumber);
        }

        public async Task<Driver?> FindByCodeAsync(string code)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task<Driver?> FindByLicenseNumberAsync(string licenseNumber)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.License.Number == licenseNumber);
        }

        public async Task<IEnumerable<Driver>> FindByStatusAsync(DriverStatus status)
        {
            return await _context.Drivers
                .Where(d => d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> FindActiveDriversAsync()
        {
            return await _context.Drivers
                .Where(d => d.IsActive)
                .OrderBy(d => d.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> FindAvailableDriversAsync()
        {
            return await _context.Drivers
                .Where(d => d.IsActive && d.Status == DriverStatus.Available)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> FindDriversWithExpiredLicensesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Drivers
                .Where(d => d.License.ExpiryDate <= now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> FindDriversWithExpiringSoonLicensesAsync(int daysThreshold = 30)
        {
            var threshold = DateTime.UtcNow.AddDays(daysThreshold);
            return await _context.Drivers
                .Where(d => d.License.ExpiryDate <= threshold && d.License.ExpiryDate > DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> FindByExperienceLevelAsync(string level)
        {
            // This would need custom logic based on experience years
            // For now, returning empty collection
            return await Task.FromResult(Enumerable.Empty<Driver>());
        }

        public async Task<bool> HasAssignedVehiclesAsync(int driverId)
        {
            // This would require checking the FleetManagement context
            // For now, returning false as we don't have direct access
            return await Task.FromResult(false);
        }

        public async Task<int> GetActiveDriversCountAsync()
        {
            return await _context.Drivers.CountAsync(d => d.IsActive);
        }
    }
}

