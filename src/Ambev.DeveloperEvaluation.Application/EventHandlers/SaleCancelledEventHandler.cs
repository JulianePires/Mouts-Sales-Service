using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.EventHandlers;

/// <summary>
/// Handles SaleCancelled domain events.
/// </summary>
public class SaleCancelledEventHandler : IDomainEventHandler<SaleCancelled>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Initializes a new instance of the SaleCancelledEventHandler.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="cacheService">Cache service for updating cached data.</param>
    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(SaleCancelled domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Handling SaleCancelled event for Sale ID: {SaleId}, Number: {SaleNumber}, Reason: {CancellationReason}",
                domainEvent.SaleId, domainEvent.SaleNumber, domainEvent.CancellationReason);

            // Invalidate relevant caches
            await _cacheService.RemoveAsync($"sale_{domainEvent.SaleId}", cancellationToken);
            await _cacheService.RemoveAsync($"customer_sales_{domainEvent.CustomerId}", cancellationToken);
            await _cacheService.RemoveAsync($"branch_sales_{domainEvent.BranchId}", cancellationToken);
            await _cacheService.RemoveAsync("sales_summary", cancellationToken);

            // Log business metrics
            _logger.LogInformation("Sale cancelled - Original Amount: {OriginalAmount:C}, Items: {OriginalItemCount}, Reason: {CancellationReason}",
                domainEvent.OriginalTotalAmount, domainEvent.OriginalItemCount, domainEvent.CancellationReason);

            // Here you could add additional business logic like:
            // - Restoring inventory quantities
            // - Processing refunds
            // - Updating customer loyalty points
            // - Sending cancellation notifications
            // - Updating financial reports

            _logger.LogDebug("Successfully handled SaleCancelled event for Sale ID: {SaleId}", domainEvent.SaleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling SaleCancelled event for Sale ID: {SaleId}", domainEvent.SaleId);
            throw;
        }
    }
}