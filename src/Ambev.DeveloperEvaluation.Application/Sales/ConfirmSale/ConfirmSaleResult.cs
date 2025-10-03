namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

/// <summary>
/// Represents the result of a confirmed sale operation.
/// </summary>
public class ConfirmSaleResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the confirmed sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number for identification.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount of the confirmed sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets when the sale was confirmed.
    /// </summary>
    public DateTime ConfirmedAt { get; set; }

    /// <summary>
    /// Gets or sets the confirmation message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}