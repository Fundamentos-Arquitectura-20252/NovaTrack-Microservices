using SharedKernel.Domain.Model;
using Maintenance.API.Domain.Model.Aggregates;
namespace Maintenance.API.Domain.Model.Commands
{
public record ScheduleMaintenanceCommand(
int VehicleId,
string Description,
MaintenanceType Type,
decimal EstimatedCost,
DateTime ScheduledDate,
string ServiceProvider,
int Priority = 3
) : ICommand<int>;
}