using SharedKernel.Domain.Model;
using Maintenance.API.Interfaces.REST.Resources;
using Maintenance.API.Domain.Model.Aggregates;
namespace Maintenance.API.Domain.Model.Queries
{
public record GetMaintenanceByTypeQuery(MaintenanceType Type) : IQuery<IEnumerable<MaintenanceRecordResource>>;
}