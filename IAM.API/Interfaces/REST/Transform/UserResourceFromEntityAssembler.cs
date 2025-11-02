using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Model.Commands;
using IAM.API.Interfaces.REST.Resources;

// UserResourceFromEntityAssembler.cs
namespace IAM.API.Interfaces.REST.Transform
{
    public static class UserResourceFromEntityAssembler
    {
        public static UserResource ToResourceFromEntity(User entity)
        {
            return new UserResource(
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Email,
                entity.Role,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            );
        }
    }
}