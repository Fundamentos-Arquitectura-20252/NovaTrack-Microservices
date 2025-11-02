using SharedKernel.Domain.Model;

// SignInUserCommand.cs
namespace IAM.API.Domain.Model.Commands
{
    public record SignInUserCommand(
        string Email,
        string Password
    ) : ICommand<int>;
}