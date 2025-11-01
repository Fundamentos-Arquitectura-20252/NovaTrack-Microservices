using FleetManagement.API.Domain.Model.Aggregates;
using FleetManagement.API.Domain.Model.ValueObjects;
using SharedKernel.Domain.Repositories;

// IVehicleRepository.cs
namespace FleetManagement.API.Domain.Repositories
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        Task<bool> ExistsByLicensePlateAsync(string licensePlate);
        Task<Vehicle?> FindByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Vehicle>> FindByFleetIdAsync(int fleetId);
        Task<IEnumerable<Vehicle>> FindByDriverIdAsync(int driverId);
        Task<IEnumerable<Vehicle>> FindByStatusAsync(VehicleStatus status);
        Task<IEnumerable<Vehicle>> FindActiveVehiclesAsync();
        Task<IEnumerable<Vehicle>> FindVehiclesInMaintenanceAsync();
        Task<IEnumerable<Vehicle>> FindVehiclesDueForServiceAsync();
        Task<Vehicle?> FindByIdWithFleetAsync(int id);
        Task<IEnumerable<Vehicle>> FindUnassignedVehiclesAsync();
    }
}