using SharedKernel.Domain.Model.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SharedKernel.Domain.Model.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Dispatching domain event {EventType} with ID {EventId}", 
                    domainEvent.EventType, domainEvent.EventId);
                await _mediator.Publish(domainEvent, cancellationToken);
                _logger.LogInformation("Successfully dispatched domain event {EventType}", domainEvent.EventType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error dispatching domain event {EventType} with ID {EventId}", 
                    domainEvent.EventType, domainEvent.EventId);
                throw;
            }
        }

        public async Task DispatchAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default)
        {
            foreach (var domainEvent in domainEvents)
            {
                await DispatchAsync(domainEvent, cancellationToken);
            }
        }
    }
}
