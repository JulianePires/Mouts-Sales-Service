namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Represents the result of cancelling a sale.
/// </summary>
/// <remarks>
/// This class encapsulates the details of a successfully cancelled sale,
/// including the sale information and cancellation details.
/// </remarks>
public class CancelSaleResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the cancelled sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number for identification and tracking.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the cancellation status (should be true).
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the total amount of the cancelled sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the number of items that were cancelled.
    /// </summary>
    public int CancelledItemsCount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the sale was cancelled.
    /// </summary>
    public DateTime CancelledAt { get; set; }

    /// <summary>
    /// Gets or sets a message indicating the result of the cancellation.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}