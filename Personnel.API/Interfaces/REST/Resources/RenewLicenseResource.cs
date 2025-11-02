namespace Personnel.API.Interfaces.REST.Resources
{
    public record RenewLicenseResource(
        string NewLicenseNumber,
        DateTime NewExpiryDate
    );
}
