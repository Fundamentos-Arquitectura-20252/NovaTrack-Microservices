using MediatR;
using FleetManagement.API.Domain.Model.Queries;
using FleetManagement.API.Domain.Repositories;
using FleetManagement.API.Interfaces.REST.Resources;
using FleetManagement.API.Interfaces.REST.Transform;
using FleetManagement.API.Domain.Model.ValueObjects;

// GetFleetByTypeQueryHandler.cs
namespace FleetManagement.API.Application.Internal.QueryServices
{
    public class GetFleetByTypeQueryHandler : IRequestHandler<GetFleetByTypeQuery, IEnumerable<FleetResource>>
    {
        private readonly IFleetRepository _fleetRepository;

        public GetFleetByTypeQueryHandler(IFleetRepository fleetRepository)
        {
            _fleetRepository = fleetRepository;
        }

        public async Task<IEnumerable<FleetResource>> Handle(GetFleetByTypeQuery request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<FleetType>(request.Type, true, out var fleetType))
                return Enumerable.Empty<FleetResource>();

            var fleets = await _fleetRepository.FindByTypeAsync(fleetType);
            return fleets.Select(FleetResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}
