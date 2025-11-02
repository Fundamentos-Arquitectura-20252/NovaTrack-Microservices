using MediatR;
using Analytics.API.Domain.Model.Queries;
using Analytics.API.Interfaces.REST.Resources;
using Flota365.Platform.API.FleetManagement.Domain.Repositories;

namespace Analytics.API.Application.Internal.QueryServices
{
    public class GetActiveVehiclesQueryHandler : IRequestHandler<GetActiveVehiclesQuery, IEnumerable<ActiveVehicleResource>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetActiveVehiclesQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<ActiveVehicleResource>> Handle(GetActiveVehiclesQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.FindActiveVehiclesAsync();
            
            return vehicles.Take(10).Select(v => new ActiveVehicleResource(
                Id: v.Id,
                LicensePlate: v.LicensePlate.Value,
                Model: $"{v.Brand} {v.Model} {v.Year}",
                DriverName: "No asignado",
                Status: v.Status.ToString(),
                FleetName: v.Fleet?.Name ?? "Sin flota",
                LastUpdate: v.UpdatedAt
            ));
        }
    }
}