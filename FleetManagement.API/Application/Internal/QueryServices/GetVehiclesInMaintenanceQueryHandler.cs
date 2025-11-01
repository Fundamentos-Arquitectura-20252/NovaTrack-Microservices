using FleetManagement.API.Domain.Model.Aggregates;
using MediatR;
    using FleetManagement.API.Domain.Model.Queries;
    using FleetManagement.API.Domain.Repositories;
    using FleetManagement.API.Interfaces.REST.Resources;
    using FleetManagement.API.Interfaces.REST.Transform;
    
    namespace FleetManagement.API.Application.Internal.QueryServices
    {
        public class GetVehiclesInMaintenanceQueryHandler : IRequestHandler<GetVehiclesInMaintenanceQuery, IEnumerable<VehicleResource>>
        {
            private readonly IVehicleRepository _vehicleRepository;
    
            public GetVehiclesInMaintenanceQueryHandler(IVehicleRepository vehicleRepository)
            {
                _vehicleRepository = vehicleRepository;
            }
    
            public async Task<IEnumerable<VehicleResource>> Handle(GetVehiclesInMaintenanceQuery request, CancellationToken cancellationToken)
            {
                var vehicles = await _vehicleRepository.FindVehiclesInMaintenanceAsync();
                return vehicles.Select<Vehicle, VehicleResource>(vehicle => VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle));
            }
        }
    }