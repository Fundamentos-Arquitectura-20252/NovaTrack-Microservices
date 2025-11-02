using SharedKernel.Domain.Model;
using Maintenance.API.Domain.Model.Aggregates;
namespace Maintenance.API.Domain.Model.Commands
{
public record CreateServiceRecordCommand(
int VehicleId,
ServiceType ServiceType,
string Description,
decimal Cost,
DateTime ServiceDate,
int MileageAtService,
string ServiceProvider,
string TechnicianName,
string PartsUsed,
string Notes
) : ICommand<int>;
}