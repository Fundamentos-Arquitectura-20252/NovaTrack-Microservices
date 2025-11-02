using MediatR;
using Personnel.API.Domain.Model.Queries;
using Personnel.API.Domain.Repositories;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Interfaces.REST.Transform;

namespace Personnel.API.Application.Internal.QueryServices
{
    public class GetDriversWithExpiringSoonLicensesQueryHandler : IRequestHandler<GetDriversWithExpiringSoonLicensesQuery, IEnumerable<DriverResource>>
    {
        private readonly IDriverRepository _driverRepository;

        public GetDriversWithExpiringSoonLicensesQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<IEnumerable<DriverResource>> Handle(GetDriversWithExpiringSoonLicensesQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _driverRepository.FindDriversWithExpiringSoonLicensesAsync(request.DaysThreshold);
            return drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}