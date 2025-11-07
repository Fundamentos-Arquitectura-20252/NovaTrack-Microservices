using Analytics.API.Application.External.DTO;

namespace Analytics.API.Application.External.Services;

public interface IFleetServiceClient
{
    Task<IEnumerable<VehicleDto>> GetActiveVehiclesAsync();
    Task<int> GetVehiclesInMaintenanceCountAsync();
    
    Task<IEnumerable<FleetDto>> GetFleetsAsync();
}