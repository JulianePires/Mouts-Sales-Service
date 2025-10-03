using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service for processing domain events.
/// </summary>
public interface IDomainEventService
{
    /// <summary>
    /// Processes a collection of domain events.
    /// </summary>
    /// <param name="events">The events to process.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the async operation.</returns>
    Task ProcessEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default);
}