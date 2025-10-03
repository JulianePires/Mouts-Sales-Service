using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Base class for all domain events in the system.
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Gets the unique identifier for the event.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the version of the event for schema evolution.
    /// </summary>
    public virtual string Version => "1.0";
}