using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Interface for publishing domain events to external systems.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes a domain event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event to publish.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : DomainEvent;

    /// <summary>
    /// Publishes multiple domain events asynchronously.
    /// </summary>
    /// <param name="domainEvents">The domain events to publish.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for handling domain events.
/// </summary>
/// <typeparam name="T">The type of domain event to handle.</typeparam>
public interface IDomainEventHandler<T> where T : DomainEvent
{
    /// <summary>
    /// Handles the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(T domainEvent, CancellationToken cancellationToken = default);
}