using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Services;

/// <summary>
/// Service for handling domain events and publishing them to external systems.
/// </summary>
public class DomainEventService : IDomainEventService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DomainEventService> _logger;

    /// <summary>
    /// Initializes a new instance of the domain event service.
    /// </summary>
    /// <param name="eventPublisher">Event publisher for external messaging.</param>
    /// <param name="serviceProvider">Service provider for resolving handlers.</param>
    /// <param name="logger">Logger instance.</param>
    public DomainEventService(
        IEventPublisher eventPublisher,
        IServiceProvider serviceProvider,
        ILogger<DomainEventService> logger)
    {
        _eventPublisher = eventPublisher;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Processes and publishes a collection of domain events.
    /// </summary>
    /// <param name="events">The domain events to process.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProcessEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default)
    {
        var eventList = events.ToList();
        if (!eventList.Any())
        {
            return;
        }

        _logger.LogInformation("Processing {EventCount} domain events", eventList.Count);

        // Handle events locally first
        foreach (var domainEvent in eventList)
        {
            await HandleEventLocally(domainEvent, cancellationToken);
        }

        // Publish events to external systems
        try
        {
            await _eventPublisher.PublishAsync(eventList, cancellationToken);
            _logger.LogInformation("Successfully published {EventCount} domain events", eventList.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish domain events to external systems");
            // In a production system, you might want to implement retry logic
            // or store failed events for later processing
        }
    }

    /// <summary>
    /// Handles a single domain event locally using registered handlers.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task HandleEventLocally(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var eventType = domainEvent.GetType();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

        try
        {
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var handleMethod = handlerType.GetMethod("HandleAsync");
                if (handleMethod != null)
                {
                    var task = (Task)handleMethod.Invoke(handler, new object[] { domainEvent, cancellationToken })!;
                    await task;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling domain event {EventType} locally", eventType.Name);
            // Don't throw here to allow other events to be processed
        }
    }
}