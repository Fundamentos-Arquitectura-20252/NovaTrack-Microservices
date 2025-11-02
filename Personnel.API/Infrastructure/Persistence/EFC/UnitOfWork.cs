using SharedKernel.Domain.Repositories;

namespace Personnel.API.Infrastructure.Persistence.EFC;

public class UnitOfWork : IUnitOfWork
{
    private readonly PersonnelDbContext _context;

    public UnitOfWork(PersonnelDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}