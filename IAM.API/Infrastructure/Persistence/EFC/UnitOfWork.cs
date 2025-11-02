using SharedKernel.Domain.Repositories;

namespace IAM.API.Infrastructure.Persistence.EFC;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAMDbContext _context;

    public UnitOfWork(IAMDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}