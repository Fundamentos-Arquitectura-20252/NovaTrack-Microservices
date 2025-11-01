using FleetManagement.API.Domain.Model.Aggregates;
using FleetManagement.API.Domain.Model.ValueObjects;
using SharedKernel.Domain.Repositories;

// IFleetRepository.cs
namespace FleetManagement.API.Domain.Repositories
{
    public interface IFleetRepository : IBaseRepository<Fleet>
    {
        Task<bool> ExistsByCodeAsync(string code);
        Task<Fleet?> FindByCodeAsync(string code);
        Task<IEnumerable<Fleet>> FindByTypeAsync(FleetType type);
        Task<IEnumerable<Fleet>> FindActiveFleetAsync();
        Task<Fleet?> FindByIdWithVehiclesAsync(int id);
    }
}
