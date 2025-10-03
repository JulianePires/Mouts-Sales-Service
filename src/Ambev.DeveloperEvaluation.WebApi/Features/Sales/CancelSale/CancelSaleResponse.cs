namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Response model for CancelSale operation
/// </summary>
public class CancelSaleResponse
{
    /// <summary>
    /// The unique identifier of the cancelled sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number for identification
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Whether the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// The total amount of the sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Number of items that were cancelled
    /// </summary>
    public int CancelledItemsCount { get; set; }

    /// <summary>
    /// When the sale was cancelled
    /// </summary>
    public DateTime CancelledAt { get; set; }

    /// <summary>
    /// Confirmation message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}