namespace IAM.API.Interfaces.REST.Resources
{
    public record ChangePasswordResource(
        string CurrentPassword,
        string NewPassword,
        string ConfirmPassword
    );
}