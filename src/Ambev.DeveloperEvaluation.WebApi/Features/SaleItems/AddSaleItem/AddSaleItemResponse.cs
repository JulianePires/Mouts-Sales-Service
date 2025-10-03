namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.AddSaleItem;

/// <summary>
/// Response model for AddSaleItem operation
/// </summary>
public class AddSaleItemResponse
{
    /// <summary>
    /// The unique identifier of the created sale item
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the sale
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The unique identifier of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The name of the product
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity added
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price applied
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The discount applied to this item
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// The total price for this item
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// The updated total amount of the sale
    /// </summary>
    public decimal SaleTotalAmount { get; set; }

    /// <summary>
    /// Confirmation message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}