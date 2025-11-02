using SharedKernel.Domain.Model;


namespace IAM.API.Domain.Model.Commands
{
    public record DeactivateUserCommand(
        int UserId
    ) : ICommand<bool>;
}