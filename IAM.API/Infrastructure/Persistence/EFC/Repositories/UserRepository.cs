using Microsoft.EntityFrameworkCore;
using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Repositories;

namespace IAM.API.Infrastructure.Persistence.EFC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IAMDbContext _context;

        public UserRepository(IAMDbContext context)
        {
            _context = context;
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }

        public void Remove(User entity)
        {
            _context.Users.Remove(entity);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> FindActiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<User?> FindByEmailAndPasswordAsync(string email, string passwordHash)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash && u.IsActive);
        }
    }
}