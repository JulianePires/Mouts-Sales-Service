namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ConfirmSale;

/// <summary>
/// Response model for ConfirmSale operation
/// </summary>
public class ConfirmSaleResponse
{
    /// <summary>
    /// The unique identifier of the confirmed sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number for identification
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The total amount of the confirmed sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// When the sale was confirmed
    /// </summary>
    public DateTime ConfirmedAt { get; set; }

    /// <summary>
    /// Confirmation message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}