using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Model.Commands;
using IAM.API.Interfaces.REST.Resources;


// UpdateProfileCommandFromResourceAssembler.cs
namespace IAM.API.Interfaces.REST.Transform
{
    public static class UpdateProfileCommandFromResourceAssembler
    {
        public static UpdateUserProfileCommand ToCommandFromResource(int userId, UpdateProfileResource resource)
        {
            return new UpdateUserProfileCommand(
                userId,
                resource.FirstName,
                resource.LastName,
                resource.Email
            );
        }
    }
}
