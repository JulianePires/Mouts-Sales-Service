namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

/// <summary>
/// Response model for AddSaleItem operation.
/// </summary>
/// <remarks>
/// This class represents the response returned after successfully adding an item to a sale.
/// It includes the newly created sale item information and updated sale totals.
/// </remarks>
public class AddSaleItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the newly created sale item.
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product ID that was added.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product added.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price used for the item.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied to the item.
    /// Calculated based on quantity: 4-9 units = 10%, 10-20 units = 20%.
    /// </summary>
    public decimal DiscountPercent { get; set; }

    /// <summary>
    /// Gets or sets the total price for the item after discount application.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Gets or sets the updated total amount for the entire sale.
    /// </summary>
    public decimal SaleTotalAmount { get; set; }
}