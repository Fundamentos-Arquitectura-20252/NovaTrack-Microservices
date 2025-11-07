using Analytics.API.Application.External.Services;
using MediatR;
using Analytics.API.Domain.Model.Queries;
using Analytics.API.Interfaces.REST.Resources;

namespace Analytics.API.Application.Internal.QueryServices
{
    public class GetFleetSummaryQueryHandler : IRequestHandler<GetFleetSummaryQuery, FleetSummaryResource>
    {
        private readonly IFleetServiceClient  _fleetRepository;

        public GetFleetSummaryQueryHandler(IFleetServiceClient  fleetRepository)
        {
            _fleetRepository = fleetRepository;
        }

        public async Task<FleetSummaryResource> Handle(GetFleetSummaryQuery request, CancellationToken cancellationToken)
        {
            var fleets = await _fleetRepository.GetFleetsAsync();
            
            var primaryFleet = fleets.FirstOrDefault(f => f.Type == "Primary");
            var secondaryFleet = fleets.FirstOrDefault(f => f.Type == "Secondary");
            var externalFleet = fleets.FirstOrDefault(f => f.Type == "External");

            return new FleetSummaryResource(
                TotalFleets: fleets.Count(),
                PrimaryFleetVehicles: primaryFleet?.ActiveVehicles ?? 0,
                SecondaryFleetVehicles: secondaryFleet?.ActiveVehicles ?? 0,
                ExternalFleetVehicles: externalFleet?.ActiveVehicles ?? 0,
                PrimaryFleetEfficiency: primaryFleet?.Efficiency ?? 100,
                SecondaryFleetEfficiency: secondaryFleet?.Efficiency ?? 100,
                ExternalFleetEfficiency: externalFleet?.Efficiency ?? 100,
                OverallEfficiency: 93.5,
                PrimaryFleetTrend: "↑ 2%",
                SecondaryFleetTrend: "→ 0%",
                ExternalFleetTrend: "↓ 1%"
            );
        }
    }
}