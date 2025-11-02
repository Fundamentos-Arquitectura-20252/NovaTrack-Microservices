using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Model.Commands;
using IAM.API.Interfaces.REST.Resources;



// SignInCommandFromResourceAssembler.cs
namespace IAM.API.Interfaces.REST.Transform
{
    public static class SignInCommandFromResourceAssembler
    {
        public static SignInUserCommand ToCommandFromResource(SignInResource resource)
        {
            return new SignInUserCommand(
                resource.Email,
                resource.Password
            );
        }
    }
}

