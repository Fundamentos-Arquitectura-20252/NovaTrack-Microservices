using MediatR;
using Analytics.API.Domain.Model.Queries;
using Analytics.API.Interfaces.REST.Resources;
using Flota365.Platform.API.FleetManagement.Domain.Repositories;

namespace Analytics.API.Application.Internal.QueryServices
{
    public class GetFleetSummaryQueryHandler : IRequestHandler<GetFleetSummaryQuery, FleetSummaryResource>
    {
        private readonly IFleetRepository _fleetRepository;

        public GetFleetSummaryQueryHandler(IFleetRepository fleetRepository)
        {
            _fleetRepository = fleetRepository;
        }

        public async Task<FleetSummaryResource> Handle(GetFleetSummaryQuery request, CancellationToken cancellationToken)
        {
            var fleets = await _fleetRepository.ListAsync();
            
            var primaryFleet = fleets.FirstOrDefault(f => f.Type == FleetManagement.Domain.Model.ValueObjects.FleetType.Primary);
            var secondaryFleet = fleets.FirstOrDefault(f => f.Type == FleetManagement.Domain.Model.ValueObjects.FleetType.Secondary);
            var externalFleet = fleets.FirstOrDefault(f => f.Type == FleetManagement.Domain.Model.ValueObjects.FleetType.External);

            return new FleetSummaryResource(
                TotalFleets: fleets.Count(),
                PrimaryFleetVehicles: primaryFleet?.GetActiveVehicleCount() ?? 0,
                SecondaryFleetVehicles: secondaryFleet?.GetActiveVehicleCount() ?? 0,
                ExternalFleetVehicles: externalFleet?.GetActiveVehicleCount() ?? 0,
                PrimaryFleetEfficiency: primaryFleet?.GetPerformanceRate() ?? 100,
                SecondaryFleetEfficiency: secondaryFleet?.GetPerformanceRate() ?? 100,
                ExternalFleetEfficiency: externalFleet?.GetPerformanceRate() ?? 100,
                OverallEfficiency: 93.5,
                PrimaryFleetTrend: "↑ 2%",
                SecondaryFleetTrend: "→ 0%",
                ExternalFleetTrend: "↓ 1%"
            );
        }
    }
}