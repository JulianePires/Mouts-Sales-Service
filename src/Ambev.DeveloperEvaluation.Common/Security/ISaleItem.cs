namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Defines the contract for representing a sale item in the system.
/// </summary>
public interface ISaleItem
{
    /// <summary>
    /// Gets the unique identifier of the sale item.
    /// </summary>
    /// <returns>The Sale Item ID.</returns>
    public Guid Id { get; }
    
    /// <summary>
    /// Gets the unique identifier for the sale associated with this item.
    /// </summary>
    /// <returns>The Sale ID.</returns>
    public Guid SaleId { get; }
    
    /// <summary>
    /// Gets the product associated with the sale item.
    /// </summary>
    /// <returns>The product information.</returns>
    public IProduct Product { get; }
    
    /// <summary>
    /// Gets the quantity of the product sold in this item.
    /// </summary>
    /// <returns>The quantity sold.</returns>
    public int Quantity { get; }
    
    /// <summary>
    /// Gets the discount percentage applied to this sale item, if any.
    /// </summary>
    /// <returns>The discount percentage (0-100).</returns>
    public decimal DiscountPercent { get; }
    
    /// <summary>
    /// Gets the price of the product at the time of sale.
    /// </summary>
    /// <returns>The unit price of the product.</returns>
    public decimal UnitPrice { get; }
    
    /// <summary>
    /// Gets the total price for this sale item after discount.
    /// </summary>
    /// <returns>The total price for the sale item.</returns>
    public decimal TotalPrice { get; }
    
    /// <summary>
    /// Gets the cancellation status of the sale item.
    /// </summary>
    /// <returns>True if the item is cancelled; otherwise, false.</returns>
    public bool IsCancelled { get; }
}