using MediatR;
using Personnel.API.Interfaces.REST.Resources;

namespace Personnel.API.Domain.Model.Queries
{
    public class GetAvailableDriversQuery : IRequest<IEnumerable<DriverResource>>
    {
    }
}