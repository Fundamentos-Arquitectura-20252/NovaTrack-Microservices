using MediatR;
using FleetManagement.API.Domain.Model.Queries;
using FleetManagement.API.Domain.Repositories;
using FleetManagement.API.Interfaces.REST.Resources;
using FleetManagement.API.Interfaces.REST.Transform;

// GetAllFleetsQueryHandler.cs
namespace FleetManagement.API.Application.Internal.QueryServices
{
    public class GetAllFleetsQueryHandler : IRequestHandler<GetAllFleetsQuery, IEnumerable<FleetResource>>
    {
        private readonly IFleetRepository _fleetRepository;

        public GetAllFleetsQueryHandler(IFleetRepository fleetRepository)
        {
            _fleetRepository = fleetRepository;
        }

        public async Task<IEnumerable<FleetResource>> Handle(GetAllFleetsQuery request, CancellationToken cancellationToken)
        {
            var fleets = await _fleetRepository.ListAsync();
            return fleets.Select(FleetResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}
