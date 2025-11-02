using SharedKernel.Domain.Model;
using IAM.API.Interfaces.REST.Resources;

namespace IAM.API.Domain.Model.Queries
{
    public record GetAllUsersQuery() : IQuery<IEnumerable<UserResource>>;
}