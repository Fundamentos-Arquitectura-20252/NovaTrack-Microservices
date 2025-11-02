using SharedKernel.Domain.Model;

namespace IAM.API.Domain.Model.Commands
{
    public record UpdateUserProfileCommand(
        int UserId,
        string FirstName,
        string LastName,
        string Email
    ) : ICommand<bool>;
}