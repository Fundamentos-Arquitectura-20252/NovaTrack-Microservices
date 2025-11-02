using MediatR;
using System.Collections.Generic;
using Personnel.API.Interfaces.REST.Resources;

namespace Personnel.API.Domain.Model.Queries
{
    public record GetActiveDriversQuery() : IRequest<IEnumerable<DriverResource>>;
}