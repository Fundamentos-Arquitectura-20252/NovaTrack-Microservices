using MediatR;
using Personnel.API.Domain.Model.Queries;
using Personnel.API.Domain.Repositories;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Interfaces.REST.Transform;

namespace Personnel.API.Application.Internal.QueryServices
{
    public class GetActiveDriversQueryHandler : IRequestHandler<GetActiveDriversQuery, IEnumerable<DriverResource>>
    {
        private readonly IDriverRepository _driverRepository;

        public GetActiveDriversQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<IEnumerable<DriverResource>> Handle(GetActiveDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _driverRepository.FindActiveDriversAsync();
            return drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}