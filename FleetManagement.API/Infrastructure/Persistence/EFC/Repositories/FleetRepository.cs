using Microsoft.EntityFrameworkCore;
using FleetManagement.API.Domain.Model.Aggregates;
using FleetManagement.API.Domain.Model.ValueObjects;
using FleetManagement.API.Domain.Repositories;

// FleetRepository.cs
namespace FleetManagement.API.Infrastructure.Persistence.EFC.Repositories
{
    public class FleetRepository : IFleetRepository
    {
        private readonly FleetDbContext _context;

        public FleetRepository(FleetDbContext context)
        {
            _context = context;
        }

        public async Task<Fleet?> FindByIdAsync(int id)
        {
            return await _context.Fleets.FindAsync(id);
        }

        public async Task<IEnumerable<Fleet>> ListAsync()
        {
            return await _context.Fleets
                .Include(f => f.Vehicles)
                .ToListAsync();
        }

        public async Task AddAsync(Fleet entity)
        {
            await _context.Fleets.AddAsync(entity);
        }

        public void Update(Fleet entity)
        {
            _context.Fleets.Update(entity);
        }

        public void Remove(Fleet entity)
        {
            _context.Fleets.Remove(entity);
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _context.Fleets.AnyAsync(f => f.Code == code);
        }

        public async Task<Fleet?> FindByCodeAsync(string code)
        {
            return await _context.Fleets
                .Include(f => f.Vehicles)
                .FirstOrDefaultAsync(f => f.Code == code);
        }

        public async Task<IEnumerable<Fleet>> FindByTypeAsync(FleetType type)
        {
            return await _context.Fleets
                .Include(f => f.Vehicles)
                .Where(f => f.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fleet>> FindActiveFleetAsync()
        {
            return await _context.Fleets
                .Include(f => f.Vehicles)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<Fleet?> FindByIdWithVehiclesAsync(int id)
        {
            return await _context.Fleets
                .Include(f => f.Vehicles)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
