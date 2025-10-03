using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Base class for entities that can raise domain events.
/// </summary>
public abstract class AggregateRoot : BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the list of domain events raised by this aggregate.
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to be published.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Removes a specific domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to remove.</param>
    protected void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from this aggregate.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}