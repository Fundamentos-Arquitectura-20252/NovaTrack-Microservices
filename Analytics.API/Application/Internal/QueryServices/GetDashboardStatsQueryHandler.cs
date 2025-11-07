using Analytics.API.Application.External.Services;
using MediatR;
using Analytics.API.Domain.Model.Queries;
using Analytics.API.Interfaces.REST.Resources;

namespace Analytics.API.Application.Internal.QueryServices
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsResource>
    {
        private readonly IFleetServiceClient _fleetClient;          // MS Fleet
        private readonly IPersonnelServiceClient _personnelClient;  // MS Personnel

        public GetDashboardStatsQueryHandler(IFleetServiceClient fleetClient, IPersonnelServiceClient personnelClient)
        {
            _fleetClient = fleetClient;
            _personnelClient = personnelClient;
        }

        public async Task<DashboardStatsResource> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _fleetClient.GetActiveVehiclesAsync();
            var totalVehicles = vehicles.Count();
        
            var activeDrivers = await _personnelClient.GetActiveDriversCountAsync();
            var vehiclesInMaintenance = vehicles.Count(v => v.Status == "Maintenance");

            var workingVehicles = totalVehicles - vehiclesInMaintenance;
            var fleetEfficiency = totalVehicles > 0 ? (double)workingVehicles / totalVehicles * 100 : 0;

            return new DashboardStatsResource(
                TotalVehicles: totalVehicles,
                ActiveDrivers: activeDrivers,
                VehiclesInMaintenance: vehiclesInMaintenance,
                FleetEfficiency: Math.Round(fleetEfficiency, 1),
                TotalVehiclesChange: "↑ 2% esta semana",
                ActiveDriversChange: "↓ 1% esta semana",
                MaintenanceChange: "↑ 3% esta semana",
                EfficiencyChange: "↑ 5% este mes"
            );
        }
    }

}