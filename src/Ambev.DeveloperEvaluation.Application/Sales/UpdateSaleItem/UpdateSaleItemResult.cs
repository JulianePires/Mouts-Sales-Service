namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Response model for UpdateSaleItem operation.
/// </summary>
/// <remarks>
/// This class represents the response returned after successfully updating a sale item quantity.
/// It includes the updated sale item information and recalculated totals.
/// </remarks>
public class UpdateSaleItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the updated sale item.
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product ID of the updated item.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the updated quantity of the item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the item.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the recalculated discount percentage for the item.
    /// Based on new quantity: 4-9 units = 10%, 10-20 units = 20%.
    /// </summary>
    public decimal DiscountPercent { get; set; }

    /// <summary>
    /// Gets or sets the recalculated total price for the item after discount.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Gets or sets the updated total amount for the entire sale.
    /// </summary>
    public decimal SaleTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the difference in quantity (positive for increases, negative for decreases).
    /// </summary>
    public int QuantityDifference { get; set; }
}