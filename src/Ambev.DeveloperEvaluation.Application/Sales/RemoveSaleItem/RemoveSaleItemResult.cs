namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Response model for RemoveSaleItem operation.
/// </summary>
/// <remarks>
/// This class represents the response returned after successfully removing a sale item.
/// It includes information about the removed item and updated sale totals.
/// The item is marked as cancelled rather than deleted to maintain audit trail.
/// </remarks>
public class RemoveSaleItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the removed sale item.
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product ID of the removed item.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity that was removed from the sale.
    /// This quantity is returned to product inventory.
    /// </summary>
    public int RemovedQuantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the removed item.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the total price that was removed from the sale.
    /// </summary>
    public decimal RemovedTotalPrice { get; set; }

    /// <summary>
    /// Gets or sets the updated total amount for the entire sale after item removal.
    /// </summary>
    public decimal SaleTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the item was successfully cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}