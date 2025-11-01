using SharedKernel.Domain.Model.Events;

namespace SharedKernel.Domain.Model
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
        protected void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;
    }
}
