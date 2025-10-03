using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.EventHandlers;

/// <summary>
/// Handles SaleCreated domain events.
/// </summary>
public class SaleCreatedEventHandler : IDomainEventHandler<SaleCreated>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Initializes a new instance of the SaleCreatedEventHandler.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="cacheService">Cache service for updating cached data.</param>
    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(SaleCreated domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Handling SaleCreated event for Sale ID: {SaleId}, Number: {SaleNumber}",
                domainEvent.SaleId, domainEvent.SaleNumber);

            // Invalidate relevant caches
            await _cacheService.RemoveAsync($"customer_sales_{domainEvent.CustomerId}", cancellationToken);
            await _cacheService.RemoveAsync($"branch_sales_{domainEvent.BranchId}", cancellationToken);
            await _cacheService.RemoveAsync("sales_summary", cancellationToken);

            // Log business metrics
            _logger.LogInformation("Sale created - Customer: {CustomerId}, Branch: {BranchId}, Amount: {Amount:C}, Items: {ItemCount}",
                domainEvent.CustomerId, domainEvent.BranchId, domainEvent.TotalAmount, domainEvent.ItemCount);

            // Here you could add additional business logic like:
            // - Sending notifications
            // - Updating analytics/reporting systems
            // - Triggering inventory management workflows
            // - Sending confirmation emails

            _logger.LogDebug("Successfully handled SaleCreated event for Sale ID: {SaleId}", domainEvent.SaleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling SaleCreated event for Sale ID: {SaleId}", domainEvent.SaleId);
            throw;
        }
    }
}