using Personnel.API.Domain.Model.Aggregates;
using MediatR;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Domain.Model.ValueObjects;

namespace Personnel.API.Domain.Model.Queries
{
    public class GetDriversByStatusQuery : IRequest<IEnumerable<DriverResource>>
    {
        public DriverStatus Status { get; }

        public GetDriversByStatusQuery(DriverStatus status)
        {
            Status = status;
        }
    }
}