using SharedKernel.Domain.Model;
using Maintenance.API.Interfaces.REST.Resources;
namespace Maintenance.API.Domain.Model.Queries
{
public record GetServiceRecordsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IQuery<IEnumerable<ServiceRecordResource>>;
}