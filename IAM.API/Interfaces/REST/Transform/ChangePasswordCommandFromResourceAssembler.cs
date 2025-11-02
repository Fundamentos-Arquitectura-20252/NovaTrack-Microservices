using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Model.Commands;
using IAM.API.Interfaces.REST.Resources;


// ChangePasswordCommandFromResourceAssembler.cs
namespace IAM.API.Interfaces.REST.Transform
{
    public static class ChangePasswordCommandFromResourceAssembler
    {
        public static ChangeUserPasswordCommand ToCommandFromResource(int userId, ChangePasswordResource resource)
        {
            return new ChangeUserPasswordCommand(
                userId,
                resource.CurrentPassword,
                resource.NewPassword
            );
        }
    }
}