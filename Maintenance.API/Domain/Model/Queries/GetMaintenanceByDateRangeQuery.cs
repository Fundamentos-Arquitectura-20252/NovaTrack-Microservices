using SharedKernel.Domain.Model;
using Maintenance.API.Interfaces.REST.Resources;
namespace Maintenance.API.Domain.Model.Queries
{
public record GetMaintenanceByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IQuery<IEnumerable<MaintenanceRecordResource>>;
}