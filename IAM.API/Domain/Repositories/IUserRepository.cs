using IAM.API.Domain.Model.Aggregates;
using SharedKernel.Domain.Repositories;

namespace IAM.API.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<IEnumerable<User>> FindActiveUsersAsync();
        Task<User?> FindByEmailAndPasswordAsync(string email, string passwordHash);
    }
}