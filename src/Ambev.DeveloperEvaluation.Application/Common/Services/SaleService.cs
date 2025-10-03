using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Common.Services;

/// <summary>
/// Service for common sale operations to reduce handler complexity.
/// </summary>
public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;

    public SaleService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Gets an active sale or throws an exception if not found or cancelled.
    /// </summary>
    /// <param name="saleId">The sale ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The active sale.</returns>
    /// <exception cref="ArgumentException">Thrown when sale is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when sale is cancelled.</exception>
    public async Task<Sale> EnsureActiveSaleAsync(Guid saleId, CancellationToken cancellationToken = default)
    {
        var sale = await _saleRepository.GetByIdAsync(saleId, cancellationToken);
        if (sale == null)
        {
            throw new ArgumentException($"Sale with ID {saleId} not found.", nameof(saleId));
        }

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot work with a cancelled sale.");
        }

        return sale;
    }

    /// <summary>
    /// Gets an active sale item from a sale or throws an exception if not found or cancelled.
    /// </summary>
    /// <param name="sale">The sale containing the item.</param>
    /// <param name="itemId">The item ID.</param>
    /// <returns>The active sale item.</returns>
    /// <exception cref="ArgumentException">Thrown when item is not found or already cancelled.</exception>
    public SaleItem EnsureActiveItem(Sale sale, Guid itemId)
    {
        var item = sale.Items.FirstOrDefault(i => i.Id == itemId && !i.IsCancelled);
        if (item == null)
        {
            throw new ArgumentException($"Sale item with ID {itemId} not found or already cancelled.", nameof(itemId));
        }

        return item;
    }
}