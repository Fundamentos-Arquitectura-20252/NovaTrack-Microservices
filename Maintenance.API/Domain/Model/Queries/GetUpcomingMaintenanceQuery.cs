using SharedKernel.Domain.Model;
using Maintenance.API.Interfaces.REST.Resources;
namespace Maintenance.API.Domain.Model.Queries
{
public record GetUpcomingMaintenanceQuery(int DaysThreshold = 7) : IQuery<IEnumerable<MaintenanceRecordResource>>;
}