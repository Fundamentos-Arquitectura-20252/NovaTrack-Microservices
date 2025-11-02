namespace IAM.API.Interfaces.REST.Resources
{
    public record SignInResource(
        string Email,
        string Password
    );
}