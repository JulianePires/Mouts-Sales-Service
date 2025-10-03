namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Response model for GetProduct operation.
/// </summary>
public class GetProductResult
{
    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the product.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The category of the product.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The image URL of the product.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The stock quantity of the product.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Indicates whether the product is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The date when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date when the product was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}