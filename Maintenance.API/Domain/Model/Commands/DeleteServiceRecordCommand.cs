using SharedKernel.Domain.Model;
namespace Maintenance.API.Domain.Model.Commands
{
public record DeleteServiceRecordCommand(
int ServiceRecordId
) : ICommand<bool>;
}