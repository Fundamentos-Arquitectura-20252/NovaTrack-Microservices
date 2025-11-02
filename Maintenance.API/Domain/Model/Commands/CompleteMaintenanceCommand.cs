using SharedKernel.Domain.Model;
namespace Maintenance.API.Domain.Model.Commands
{
public record CompleteMaintenanceCommand(
int MaintenanceId,
decimal ActualCost,
string CompletionNotes
) : ICommand<bool>;
}