using MediatR;
using Personnel.API.Domain.Repositories;
using Personnel.API.Domain.Model.Queries;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Interfaces.REST.Transform;

namespace Personnel.API.Application.Internal.QueryServices
{
    public class GetDriversByStatusQueryHandler : IRequestHandler<GetDriversByStatusQuery, IEnumerable<DriverResource>>
    {
        private readonly IDriverRepository _driverRepository;

        public GetDriversByStatusQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<IEnumerable<DriverResource>> Handle(GetDriversByStatusQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _driverRepository.FindByStatusAsync(request.Status);
            return drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}