namespace Maintenance.API.Interfaces.REST.Resources
{
    public record CompleteMaintenanceResource(
        decimal ActualCost,
        string CompletionNotes
    );
}
