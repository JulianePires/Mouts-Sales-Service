namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.UpdateSaleItem;

/// <summary>
/// Response model for update sale item operation
/// </summary>
public class UpdateSaleItemResponse
{
    /// <summary>
    /// The updated sale item ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale ID that this item belongs to
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The product ID of the sale item
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The updated quantity of the item
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the item
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The total price of the item (Quantity * UnitPrice)
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// The discount applied to this item
    /// </summary>
    public decimal Discount { get; set; }
}