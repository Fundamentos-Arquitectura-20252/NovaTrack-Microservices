using SharedKernel.Domain.Model;

// SignUpUserCommand.cs
namespace IAM.API.Domain.Model.Commands
{
    public record SignUpUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Role
    ) : ICommand<int>;
}
