using Analytics.API.Application.External.Services;
using MediatR;
using Analytics.API.Domain.Model.Queries;
using Analytics.API.Interfaces.REST.Resources;

namespace Analytics.API.Application.Internal.QueryServices
{
    public class
        GetActiveVehiclesQueryHandler : IRequestHandler<GetActiveVehiclesQuery, IEnumerable<ActiveVehicleResource>>
    {
        private readonly IFleetServiceClient _fleetClient;

        public GetActiveVehiclesQueryHandler(IFleetServiceClient fleetClient)
        {
            _fleetClient = fleetClient;
        }

        public async Task<IEnumerable<ActiveVehicleResource>> Handle(GetActiveVehiclesQuery request,
            CancellationToken cancellationToken)
        {
            var vehicles = await _fleetClient.GetActiveVehiclesAsync();

            return vehicles.Take(10).Select(v => new ActiveVehicleResource(
                Id: v.Id,
                LicensePlate: v.LicensePlate,
                Model: $"{v.Brand} {v.Model} {v.Year}",
                DriverName: v.DriverName ?? "No asignado",
                Status: v.Status,
                FleetName: v.FleetName ?? "Sin flota",
                LastUpdate: v.LastUpdate
            ));
        }
    }
}