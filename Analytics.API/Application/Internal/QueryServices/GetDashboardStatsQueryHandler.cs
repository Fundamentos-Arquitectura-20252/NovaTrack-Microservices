using MediatR;
using Analytics.API.Domain.Model.Queries;
using Analytics.API.Interfaces.REST.Resources;
using Flota365.Platform.API.FleetManagement.Domain.Repositories;
using Flota365.Platform.API.Personnel.Domain.Repositories;
using Flota365.Platform.API.Maintenance.Domain.Repositories;

namespace Analytics.API.Application.Internal.QueryServices
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsResource>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDriverRepository _driverRepository;

        public GetDashboardStatsQueryHandler(IVehicleRepository vehicleRepository, IDriverRepository driverRepository)
        {
            _vehicleRepository = vehicleRepository;
            _driverRepository = driverRepository;
        }

        public async Task<DashboardStatsResource> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.ListAsync();
            var totalVehicles = vehicles.Count();
            var activeDrivers = await _driverRepository.GetActiveDriversCountAsync();
            var vehiclesInMaintenance = vehicles.Count(v => v.Status == FleetManagement.Domain.Model.Aggregates.VehicleStatus.Maintenance);
            
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