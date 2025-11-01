using SharedKernel.Domain.Repositories;

namespace FleetManagement.API.Infrastructure.Persistence.EFC;

public class UnitOfWork : IUnitOfWork
{
    private readonly FleetDbContext _context;

    public UnitOfWork(FleetDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}