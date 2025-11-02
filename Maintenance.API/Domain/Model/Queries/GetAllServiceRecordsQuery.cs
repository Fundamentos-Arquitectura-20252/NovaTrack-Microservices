using SharedKernel.Domain.Model;
using Maintenance.API.Interfaces.REST.Resources;
namespace Maintenance.API.Domain.Model.Queries
{
public record GetAllServiceRecordsQuery() : IQuery<IEnumerable<ServiceRecordResource>>;
}