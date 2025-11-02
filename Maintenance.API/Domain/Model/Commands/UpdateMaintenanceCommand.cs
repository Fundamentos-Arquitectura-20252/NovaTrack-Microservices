using SharedKernel.Domain.Model;
using Maintenance.API.Domain.Model.Aggregates;
namespace Maintenance.API.Domain.Model.Commands
{
public record UpdateMaintenanceCommand(
int MaintenanceId,
string Description,
MaintenanceType Type,
decimal EstimatedCost,
DateTime ScheduledDate,
string ServiceProvider,
string Notes,
MaintenanceStatus Status
) : ICommand<bool>;
}