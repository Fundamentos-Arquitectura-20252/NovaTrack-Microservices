using MediatR;
using FleetManagement.API.Domain.Model.Queries;
using FleetManagement.API.Domain.Repositories;
using FleetManagement.API.Interfaces.REST.Resources;
using FleetManagement.API.Interfaces.REST.Transform;

// GetVehicleByLicensePlateQueryHandler.cs
namespace FleetManagement.API.Application.Internal.QueryServices
{
    public class GetVehicleByLicensePlateQueryHandler : IRequestHandler<GetVehicleByLicensePlateQuery, VehicleResource?>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetVehicleByLicensePlateQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<VehicleResource?> Handle(GetVehicleByLicensePlateQuery request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.FindByLicensePlateAsync(request.LicensePlate);
            return vehicle != null ? VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle) : null;
        }
    }
}
