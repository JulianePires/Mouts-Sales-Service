using Azure.Messaging.ServiceBus;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Services;

/// <summary>
/// Azure Service Bus implementation of the event publisher.
/// </summary>
public class AzureServiceBusEventPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly ServiceBusClient? _serviceBusClient;
    private readonly ServiceBusSender? _sender;
    private readonly ILogger<AzureServiceBusEventPublisher> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the Azure Service Bus event publisher.
    /// </summary>
    /// <param name="configuration">Configuration containing connection string.</param>
    /// <param name="logger">Logger instance.</param>
    public AzureServiceBusEventPublisher(IConfiguration configuration, ILogger<AzureServiceBusEventPublisher> logger)
    {
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        var connectionString = configuration.GetConnectionString("ServiceBus");
        var queueName = configuration["ServiceBus:QueueName"] ?? "domain-events-queue";

        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogWarning("Service Bus connection string not configured. Events will not be published.");
            return;
        }

        try
        {
            _serviceBusClient = new ServiceBusClient(connectionString);
            _sender = _serviceBusClient.CreateSender(queueName);
            _logger.LogInformation("Azure Service Bus event publisher initialized successfully for queue: {QueueName}", queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Azure Service Bus client");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : DomainEvent
    {
        if (_sender == null)
        {
            _logger.LogWarning("Service Bus sender not initialized. Skipping event publication.");
            return;
        }

        try
        {
            var eventData = JsonSerializer.Serialize(domainEvent, _jsonOptions);
            var message = new ServiceBusMessage(eventData)
            {
                Subject = domainEvent.GetType().Name,
                MessageId = domainEvent.Id.ToString(),
                ContentType = "application/json"
            };

            // Add custom properties for filtering and routing
            message.ApplicationProperties["EventType"] = domainEvent.GetType().Name;
            message.ApplicationProperties["EventVersion"] = domainEvent.Version;
            message.ApplicationProperties["OccurredOn"] = domainEvent.OccurredOn.ToString("O");

            await _sender.SendMessageAsync(message, cancellationToken);

            _logger.LogInformation("Published domain event {EventType} with ID {EventId}",
                domainEvent.GetType().Name, domainEvent.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish domain event {EventType} with ID {EventId}",
                domainEvent.GetType().Name, domainEvent.Id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task PublishAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        if (_sender == null)
        {
            _logger.LogWarning("Service Bus sender not initialized. Skipping events publication.");
            return;
        }

        var events = domainEvents.ToList();
        if (!events.Any())
        {
            return;
        }

        try
        {
            var messages = events.Select(domainEvent =>
            {
                var eventData = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), _jsonOptions);
                var message = new ServiceBusMessage(eventData)
                {
                    Subject = domainEvent.GetType().Name,
                    MessageId = domainEvent.Id.ToString(),
                    ContentType = "application/json"
                };

                // Add custom properties for filtering and routing
                message.ApplicationProperties["EventType"] = domainEvent.GetType().Name;
                message.ApplicationProperties["EventVersion"] = domainEvent.Version;
                message.ApplicationProperties["OccurredOn"] = domainEvent.OccurredOn.ToString("O");

                return message;
            }).ToList();

            await _sender.SendMessagesAsync(messages, cancellationToken);

            _logger.LogInformation("Published {Count} domain events", events.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish {Count} domain events", events.Count);
            throw;
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_sender != null)
        {
            await _sender.DisposeAsync();
        }

        if (_serviceBusClient != null)
        {
            await _serviceBusClient.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}