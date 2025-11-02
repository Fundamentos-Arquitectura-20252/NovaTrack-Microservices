using SharedKernel.Domain.Model;
namespace Maintenance.API.Domain.Model.Commands
{
public record CancelMaintenanceCommand(
int MaintenanceId,
string Reason
) : ICommand<bool>;
}