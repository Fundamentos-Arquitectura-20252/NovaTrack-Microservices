using MediatR;
using Personnel.API.Domain.Model.Queries;
using Personnel.API.Domain.Repositories;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Interfaces.REST.Transform;

namespace Personnel.API.Application.Internal.QueryServices
{
    public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, IEnumerable<DriverResource>>
    {
        private readonly IDriverRepository _driverRepository;

        public GetAllDriversQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<IEnumerable<DriverResource>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _driverRepository.ListAsync();
            return drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}