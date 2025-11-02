using SharedKernel.Domain.Model;
using Maintenance.API.Interfaces.REST.Resources;
namespace Maintenance.API.Domain.Model.Queries
{
public record GetRecentServiceRecordsQuery(int DaysThreshold = 30) : IQuery<IEnumerable<ServiceRecordResource>>;
}