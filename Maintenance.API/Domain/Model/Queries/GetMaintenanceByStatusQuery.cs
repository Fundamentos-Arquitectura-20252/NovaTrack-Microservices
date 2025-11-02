using SharedKernel.Domain.Model;
using Maintenance.API.Interfaces.REST.Resources;
using Maintenance.API.Domain.Model.Aggregates;
namespace Maintenance.API.Domain.Model.Queries
{
public record GetMaintenanceByStatusQuery(MaintenanceStatus Status) : IQuery<IEnumerable<MaintenanceRecordResource>>;
}