using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Model.Commands;
using IAM.API.Interfaces.REST.Resources;


// SignUpCommandFromResourceAssembler.cs
namespace IAM.API.Interfaces.REST.Transform
{
    public static class SignUpCommandFromResourceAssembler
    {
        public static SignUpUserCommand ToCommandFromResource(SignUpResource resource)
        {
            return new SignUpUserCommand(
                resource.FirstName,
                resource.LastName,
                resource.Email,
                resource.Password,
                resource.Role
            );
        }
    }
}
