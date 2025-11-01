using MediatR;
using FleetManagement.API.Domain.Model.Queries;
using FleetManagement.API.Domain.Repositories;
using FleetManagement.API.Interfaces.REST.Resources;
using FleetManagement.API.Interfaces.REST.Transform;

namespace FleetManagement.API.Application.Internal.QueryServices
{
    public class GetVehiclesDueForServiceQueryHandler : IRequestHandler<GetVehiclesDueForServiceQuery, IEnumerable<VehicleResource>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetVehiclesDueForServiceQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<VehicleResource>> Handle(GetVehiclesDueForServiceQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.FindVehiclesDueForServiceAsync();
            return vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}