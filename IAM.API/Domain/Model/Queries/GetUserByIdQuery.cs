using SharedKernel.Domain.Model;
using IAM.API.Interfaces.REST.Resources;

// GetUserByIdQuery.cs
namespace IAM.API.Domain.Model.Queries
{
    public record GetUserByIdQuery(int UserId) : IQuery<UserResource?>;
}