using MediatR;
using Personnel.API.Interfaces.REST.Resources;

namespace Personnel.API.Domain.Model.Queries
{
    public record GetDriversWithExpiringSoonLicensesQuery(int DaysThreshold) 
        : IRequest<IEnumerable<DriverResource>>;
}