using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.EventHandlers;

/// <summary>
/// Handles ItemCancelled domain events.
/// </summary>
public class ItemCancelledEventHandler : IDomainEventHandler<ItemCancelled>
{
    private readonly ILogger<ItemCancelledEventHandler> _logger;
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Initializes a new instance of the ItemCancelledEventHandler.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="cacheService">Cache service for updating cached data.</param>
    public ItemCancelledEventHandler(ILogger<ItemCancelledEventHandler> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(ItemCancelled domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Handling ItemCancelled event for Item ID: {ItemId}, Sale: {SaleNumber}, Product: {ProductName}, Reason: {CancellationReason}",
                domainEvent.ItemId, domainEvent.SaleNumber, domainEvent.ProductName, domainEvent.CancellationReason);

            // Invalidate relevant caches
            await _cacheService.RemoveAsync($"sale_{domainEvent.SaleId}", cancellationToken);
            await _cacheService.RemoveAsync($"product_sales_{domainEvent.ProductId}", cancellationToken);

            // Log business metrics
            _logger.LogInformation("Item cancelled - Product: {ProductName}, Quantity: {Quantity}, Unit Price: {UnitPrice:C}, Total: {TotalPrice:C}, Discount: {DiscountPercent}%",
                domainEvent.ProductName, domainEvent.Quantity, domainEvent.UnitPrice, domainEvent.TotalPrice, domainEvent.DiscountPercent);

            // Here you could add additional business logic like:
            // - Restoring product inventory for the cancelled quantity
            // - Adjusting product sales statistics
            // - Updating promotional campaign metrics
            // - Triggering quality control processes if cancellation reason indicates defects

            _logger.LogDebug("Successfully handled ItemCancelled event for Item ID: {ItemId}", domainEvent.ItemId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling ItemCancelled event for Item ID: {ItemId}", domainEvent.ItemId);
            throw;
        }
    }
}