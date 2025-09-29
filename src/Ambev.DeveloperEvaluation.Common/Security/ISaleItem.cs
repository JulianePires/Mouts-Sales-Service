namespace Ambev.DeveloperEvaluation.Common.Security;

public interface ISaleItem
{
    /// <summary>
    /// Gets the ID of the sale item.
    /// </summary>
    /// <returns>The Sale Item ID as string.</returns>
    public string Id { get; }
    
    /// <summary>
    /// Gets the unique identifier for the sale associated with this item.
    /// </summary>
    /// <returns>The Sale ID as string.</returns>
    public string SaleId { get; }
    
    /// <summary>
    /// Gets the product associated with the sale item.
    /// </summary>
    /// <returns>The product.</returns>
    public IProduct Product { get; }
    
    /// <summary>
    /// Gets the quantity of the product sold.
    /// </summary>
    /// <returns>The quantity as integer.</returns>
    public int Quantity { get; }
    
    /// <summary>
    /// Gets the price per unit of the product.
    /// </summary>
    /// <returns>The price per unit as decimal.</returns>
    public decimal PricePerUnit { get; }
}