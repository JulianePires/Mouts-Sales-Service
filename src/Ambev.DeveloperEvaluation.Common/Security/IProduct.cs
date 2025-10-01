namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Defines the contract for representing a product in the system.
/// </summary>
public interface IProduct
{
    /// <summary>
    /// Gets the unique identifier for the product.
    /// </summary>
    /// <returns>The Product ID as string.</returns>
    public string Id { get; }
    
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    /// <returns>The product name.</returns>
    public string Name { get; }
    
    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    /// <returns>The product price.</returns>
    public decimal Price { get; }
    
    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    /// <returns>The product description.</returns>
    public string Description { get; }
    
    /// <summary>
    /// Gets the category of the product.
    /// </summary>
    /// <returns>The product category.</returns>
    public string Category { get; }
    
    /// <summary>
    /// Gets the image URL of the product.
    /// </summary>
    /// <returns>The product image URL.</returns>
    public string Image { get; }
    
    /// <summary>
    /// Gets the stock quantity of the product.
    /// </summary>
    /// <returns>The product stock quantity.</returns>
    public int StockQuantity { get; }
    
    /// <summary>
    /// Gets the minimum stock level for the product.
    /// </summary>
    /// <returns>The minimum stock level.</returns>
    public int MinStockLevel { get; }
    
    /// <summary>
    /// Gets the active status of the product.
    /// </summary>
    /// <returns>True if the product is active; otherwise, false.</returns>
    public bool IsActive { get; }
    
    /// <summary>
    /// Gets the creation date of the product.
    /// </summary>
    /// <returns>The product creation date.</returns>
    public DateTime CreatedAt { get; }
}