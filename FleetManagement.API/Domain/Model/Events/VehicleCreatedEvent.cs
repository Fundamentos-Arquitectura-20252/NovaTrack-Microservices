using SharedKernel.Domain.Model.Events;

namespace FleetManagement.API.Domain.Model.Events
{
    public record VehicleCreatedEvent(
        int VehicleId,
        string LicensePlate,
        string Brand,
        string Model,
        DateTime CreatedAt
    ) : DomainEvent;
}
