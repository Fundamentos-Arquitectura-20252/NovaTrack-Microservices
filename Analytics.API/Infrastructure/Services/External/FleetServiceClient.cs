using Analytics.API.Application.External.DTO;
using Analytics.API.Application.External.Services;

namespace Analytics.API.Infrastructure.Services.External;

public class FleetServiceClient : IFleetServiceClient
{
    private readonly HttpClient _httpClient;

    public FleetServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<VehicleDto>> GetActiveVehiclesAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<VehicleDto>>(
            "/api/vehicles/active"
        );
    }
    public async Task<int> GetVehiclesInMaintenanceCountAsync()
    {
        return await _httpClient.GetFromJsonAsync<int>("/api/v1/vehicles/maintenance/count");
    }
    
    public async Task<IEnumerable<FleetDto>> GetFleetsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<FleetDto>>("/api/v1/fleets");
    }
    
}