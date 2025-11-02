using SharedKernel.Domain.Model;
using Maintenance.API.Domain.Model.Aggregates;
namespace Maintenance.API.Domain.Model.Commands
{
public record UpdateServiceRecordCommand(
int ServiceRecordId,
string Description,
decimal Cost,
string ServiceProvider,
string TechnicianName,
ServiceQuality Quality,
string PartsUsed,
string Notes
) : ICommand<bool>;
}