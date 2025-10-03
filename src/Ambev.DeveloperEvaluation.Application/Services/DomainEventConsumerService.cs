using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Configuration;

namespace Ambev.DeveloperEvaluation.Consumer;

/// <summary>
/// Example consumer service that processes domain events from Azure Service Bus queue.
/// This demonstrates how external services can consume events published by the application.
/// </summary>
public class DomainEventConsumerService : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<DomainEventConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly JsonSerializerOptions _jsonOptions;

    public DomainEventConsumerService(
        IConfiguration configuration,
        ILogger<DomainEventConsumerService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        var connectionString = configuration.GetConnectionString("ServiceBus");
        var queueName = configuration["ServiceBus:QueueName"] ?? "domain-events-queue";

        if (!string.IsNullOrEmpty(connectionString))
        {
            var serviceBusClient = new ServiceBusClient(connectionString);
            _processor = serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 5,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            });

            _processor.ProcessMessageAsync += ProcessMessageAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
        }

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_processor != null)
        {
            _logger.LogInformation("Starting Domain Event Consumer Service");
            await _processor.StartProcessingAsync(stoppingToken);

            // Wait until cancellation is requested
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Stopping Domain Event Consumer Service");
            await _processor.StopProcessingAsync(stoppingToken);
        }
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var eventType = args.Message.ApplicationProperties["EventType"]?.ToString();
        var messageBody = args.Message.Body.ToString();

        _logger.LogInformation("Processing event: {EventType}, MessageId: {MessageId}",
            eventType, args.Message.MessageId);

        try
        {
            using var scope = _serviceProvider.CreateScope();

            // Process based on event type
            switch (eventType)
            {
                case nameof(SaleCreated):
                    var saleCreated = JsonSerializer.Deserialize<SaleCreated>(messageBody, _jsonOptions);
                    await ProcessSaleCreatedAsync(saleCreated, scope.ServiceProvider);
                    break;

                case nameof(SaleModified):
                    var saleModified = JsonSerializer.Deserialize<SaleModified>(messageBody, _jsonOptions);
                    await ProcessSaleModifiedAsync(saleModified, scope.ServiceProvider);
                    break;

                case nameof(SaleCancelled):
                    var saleCancelled = JsonSerializer.Deserialize<SaleCancelled>(messageBody, _jsonOptions);
                    await ProcessSaleCancelledAsync(saleCancelled, scope.ServiceProvider);
                    break;

                case nameof(ItemCancelled):
                    var itemCancelled = JsonSerializer.Deserialize<ItemCancelled>(messageBody, _jsonOptions);
                    await ProcessItemCancelledAsync(itemCancelled, scope.ServiceProvider);
                    break;

                default:
                    _logger.LogWarning("Unknown event type: {EventType}", eventType);
                    break;
            }

            // Complete the message to remove it from the queue
            await args.CompleteMessageAsync(args.Message);
            _logger.LogInformation("Successfully processed event: {EventType}", eventType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing event: {EventType}", eventType);

            // Abandon the message to retry (up to max delivery count)
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private async Task ProcessSaleCreatedAsync(SaleCreated saleCreated, IServiceProvider serviceProvider)
    {
        // Example: Update external analytics system
        _logger.LogInformation("Processing SaleCreated: SaleId={SaleId}, CustomerId={CustomerId}, TotalAmount={TotalAmount}",
            saleCreated.SaleId, saleCreated.CustomerId, saleCreated.TotalAmount);

        // Add your business logic here:
        // - Update analytics database
        // - Send notification emails
        // - Update inventory forecasting
        // - Trigger loyalty program updates
        // etc.

        await Task.CompletedTask; // Replace with actual async operations
    }

    private async Task ProcessSaleModifiedAsync(SaleModified saleModified, IServiceProvider serviceProvider)
    {
        _logger.LogInformation("Processing SaleModified: SaleId={SaleId}", saleModified.SaleId);

        // Add your business logic here:
        // - Update analytics with changes
        // - Recalculate metrics
        // - Audit trail updates
        // etc.

        await Task.CompletedTask;
    }

    private async Task ProcessSaleCancelledAsync(SaleCancelled saleCancelled, IServiceProvider serviceProvider)
    {
        _logger.LogInformation("Processing SaleCancelled: SaleId={SaleId}", saleCancelled.SaleId);

        // Add your business logic here:
        // - Update inventory availability
        // - Process refunds
        // - Update customer analytics
        // - Cancel related processes
        // etc.

        await Task.CompletedTask;
    }

    private async Task ProcessItemCancelledAsync(ItemCancelled itemCancelled, IServiceProvider serviceProvider)
    {
        _logger.LogInformation("Processing ItemCancelled: SaleId={SaleId}, ProductId={ProductId}",
            itemCancelled.SaleId, itemCancelled.ProductId);

        // Add your business logic here:
        // - Update partial inventory
        // - Partial refund processing
        // - Update item-level analytics
        // etc.

        await Task.CompletedTask;
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error occurred while processing Service Bus message: {ErrorSource}",
            args.ErrorSource);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processor != null)
        {
            await _processor.CloseAsync(cancellationToken);
            await _processor.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}