namespace IAM.API.Interfaces.REST.Resources
{
    public record UpdateProfileResource(
        string FirstName,
        string LastName,
        string Email
    );
}