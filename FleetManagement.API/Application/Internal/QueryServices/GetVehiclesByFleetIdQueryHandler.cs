using MediatR;
using FleetManagement.API.Domain.Model.Queries;
using FleetManagement.API.Domain.Repositories;
using FleetManagement.API.Interfaces.REST.Resources;
using FleetManagement.API.Interfaces.REST.Transform;

// GetVehiclesByFleetIdQueryHandler.cs
namespace FleetManagement.API.Application.Internal.QueryServices
{
    public class GetVehiclesByFleetIdQueryHandler : IRequestHandler<GetVehiclesByFleetIdQuery, IEnumerable<VehicleResource>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetVehiclesByFleetIdQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<VehicleResource>> Handle(GetVehiclesByFleetIdQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.FindByFleetIdAsync(request.FleetId);
            return vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}
