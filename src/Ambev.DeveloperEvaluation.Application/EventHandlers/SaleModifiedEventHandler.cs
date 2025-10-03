using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.EventHandlers;

/// <summary>
/// Handles SaleModified domain events.
/// </summary>
public class SaleModifiedEventHandler : IDomainEventHandler<SaleModified>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Initializes a new instance of the SaleModifiedEventHandler.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="cacheService">Cache service for updating cached data.</param>
    public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(SaleModified domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Handling SaleModified event for Sale ID: {SaleId}, Number: {SaleNumber}, Modification: {ModificationType}",
                domainEvent.SaleId, domainEvent.SaleNumber, domainEvent.ModificationType);

            // Invalidate relevant caches
            await _cacheService.RemoveAsync($"sale_{domainEvent.SaleId}", cancellationToken);
            await _cacheService.RemoveAsync($"customer_sales_{domainEvent.CustomerId}", cancellationToken);
            await _cacheService.RemoveAsync($"branch_sales_{domainEvent.BranchId}", cancellationToken);
            await _cacheService.RemoveAsync("sales_summary", cancellationToken);

            // Log business metrics
            _logger.LogInformation("Sale modified - Type: {ModificationType}, Details: {ModificationDetails}, New Amount: {Amount:C}, Items: {ItemCount}",
                domainEvent.ModificationType, domainEvent.ModificationDetails, domainEvent.TotalAmount, domainEvent.ItemCount);

            // Here you could add additional business logic like:
            // - Auditing changes for compliance
            // - Updating inventory if items were added/removed
            // - Recalculating customer loyalty points
            // - Triggering fraud detection systems for significant changes

            _logger.LogDebug("Successfully handled SaleModified event for Sale ID: {SaleId}", domainEvent.SaleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling SaleModified event for Sale ID: {SaleId}", domainEvent.SaleId);
            throw;
        }
    }
}