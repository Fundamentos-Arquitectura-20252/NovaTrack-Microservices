using SharedKernel.Domain.Repositories;

namespace Maintenance.API.Infrastructure.Persistence.EFC;

public class UnitOfWork : IUnitOfWork
{
    private readonly MaintenanceDbContext _context;

    public UnitOfWork(MaintenanceDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}