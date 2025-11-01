using MediatR;
using FleetManagement.API.Domain.Model.Queries;
using FleetManagement.API.Domain.Repositories;
using FleetManagement.API.Interfaces.REST.Resources;
using FleetManagement.API.Interfaces.REST.Transform;

// GetFleetByIdQueryHandler.cs
namespace FleetManagement.API.Application.Internal.QueryServices
{
    public class GetFleetByIdQueryHandler : IRequestHandler<GetFleetByIdQuery, FleetResource?>
    {
        private readonly IFleetRepository _fleetRepository;

        public GetFleetByIdQueryHandler(IFleetRepository fleetRepository)
        {
            _fleetRepository = fleetRepository;
        }

        public async Task<FleetResource?> Handle(GetFleetByIdQuery request, CancellationToken cancellationToken)
        {
            var fleet = await _fleetRepository.FindByIdWithVehiclesAsync(request.FleetId);
            return fleet != null ? FleetResourceFromEntityAssembler.ToResourceFromEntity(fleet) : null;
        }
    }
}
