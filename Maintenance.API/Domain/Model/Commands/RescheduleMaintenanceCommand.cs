using SharedKernel.Domain.Model;
namespace Maintenance.API.Domain.Model.Commands
{
public record RescheduleMaintenanceCommand(
int MaintenanceId,
DateTime NewScheduledDate,
string Reason
) : ICommand<bool>;
}