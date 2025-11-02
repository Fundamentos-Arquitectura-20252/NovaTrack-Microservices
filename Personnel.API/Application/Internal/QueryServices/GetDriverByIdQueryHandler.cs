using MediatR;
using Personnel.API.Domain.Model.Queries;
using Personnel.API.Domain.Repositories;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Interfaces.REST.Transform;

namespace Personnel.API.Application.Internal.QueryServices
{
    public class GetDriverByIdQueryHandler : IRequestHandler<GetDriverByIdQuery, DriverResource?>
    {
        private readonly IDriverRepository _driverRepository;

        public GetDriverByIdQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<DriverResource?> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.FindByIdAsync(request.DriverId);
            return driver != null ? DriverResourceFromEntityAssembler.ToResourceFromEntity(driver) : null;
        }
    }
}