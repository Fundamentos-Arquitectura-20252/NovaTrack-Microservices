using SharedKernel.Domain.Model;


namespace IAM.API.Domain.Model.Commands
{
    public record ChangeUserPasswordCommand(
        int UserId,
        string CurrentPassword,
        string NewPassword
    ) : ICommand<bool>;
}