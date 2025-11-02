using SharedKernel.Domain.Model;
namespace Maintenance.API.Domain.Model.Commands
{
public record StartMaintenanceCommand(
int MaintenanceId
) : ICommand<bool>;
}