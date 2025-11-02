using MediatR;
using Personnel.API.Domain.Repositories;
using Personnel.API.Domain.Model.Queries;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Interfaces.REST.Transform;

namespace Personnel.API.Application.Internal.QueryServices
{
    public class GetAvailableDriversQueryHandler : IRequestHandler<GetAvailableDriversQuery, IEnumerable<DriverResource>>
    {
        private readonly IDriverRepository _driverRepository;

        public GetAvailableDriversQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<IEnumerable<DriverResource>> Handle(GetAvailableDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _driverRepository.FindAvailableDriversAsync();
            return drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}