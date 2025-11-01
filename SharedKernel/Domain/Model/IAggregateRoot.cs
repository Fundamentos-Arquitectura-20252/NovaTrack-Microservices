using SharedKernel.Domain.Model.Events;

namespace SharedKernel.Domain.Model
{
    public interface IAggregateRoot : IEntity
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void AddDomainEvent(DomainEvent domainEvent);
        void RemoveDomainEvent(DomainEvent domainEvent);
        void ClearDomainEvents();
    }
}
