using MediatR;
using FleetManagement.API.Domain.Model.Queries;
using FleetManagement.API.Domain.Repositories;
using FleetManagement.API.Interfaces.REST.Resources;
using FleetManagement.API.Interfaces.REST.Transform;

// GetActiveFleetQueryHandler.cs
namespace FleetManagement.API.Application.Internal.QueryServices
{
    public class GetActiveFleetQueryHandler : IRequestHandler<GetActiveFleetQuery, IEnumerable<FleetResource>>
    {
        private readonly IFleetRepository _fleetRepository;

        public GetActiveFleetQueryHandler(IFleetRepository fleetRepository)
        {
            _fleetRepository = fleetRepository;
        }

        public async Task<IEnumerable<FleetResource>> Handle(GetActiveFleetQuery request, CancellationToken cancellationToken)
        {
            var fleets = await _fleetRepository.FindActiveFleetAsync();
            return fleets.Select(FleetResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}
