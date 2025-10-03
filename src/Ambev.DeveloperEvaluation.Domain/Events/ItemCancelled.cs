using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a sale item is cancelled.
/// </summary>
public class ItemCancelled : DomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the cancelled item.
    /// </summary>
    public Guid ItemId { get; }

    /// <summary>
    /// Gets the unique identifier of the sale that contains the item.
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Gets the sale number for identification.
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Gets the product ID of the cancelled item.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the product name of the cancelled item.
    /// </summary>
    public string ProductName { get; }

    /// <summary>
    /// Gets the quantity that was cancelled.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the unit price of the cancelled item.
    /// </summary>
    public decimal UnitPrice { get; }

    /// <summary>
    /// Gets the total price of the cancelled item.
    /// </summary>
    public decimal TotalPrice { get; }

    /// <summary>
    /// Gets the discount percentage that was applied to the item.
    /// </summary>
    public decimal DiscountPercent { get; }

    /// <summary>
    /// Gets the reason for the item cancellation.
    /// </summary>
    public string CancellationReason { get; }

    /// <summary>
    /// Initializes a new instance of the ItemCancelled event.
    /// </summary>
    /// <param name="saleItem">The cancelled sale item.</param>
    /// <param name="saleNumber">The sale number containing the item.</param>
    /// <param name="cancellationReason">The reason for cancellation.</param>
    public ItemCancelled(SaleItem saleItem, string saleNumber, string cancellationReason = "")
    {
        ItemId = saleItem.Id;
        SaleId = saleItem.SaleId;
        SaleNumber = saleNumber;
        ProductId = saleItem.Product?.Id ?? Guid.Empty;
        ProductName = saleItem.Product?.Name ?? "Unknown Product";
        Quantity = saleItem.Quantity;
        UnitPrice = saleItem.UnitPrice;
        TotalPrice = saleItem.TotalPrice;
        DiscountPercent = saleItem.DiscountPercent;
        CancellationReason = cancellationReason;
    }
}