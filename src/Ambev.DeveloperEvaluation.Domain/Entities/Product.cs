using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system with inventory and pricing information.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Product : BaseEntity, IProduct
{
    /// <summary>
    /// Gets the name of the product.
    /// Must not be null or empty and should be unique within the system.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the price of the product.
    /// Must be greater than zero for active products.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets the description of the product.
    /// Provides detailed information about the product features and specifications.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets the category of the product.
    /// Used for product classification and filtering.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets the image URL of the product.
    /// Points to the product's main display image.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets the current stock quantity of the product.
    /// Must not be negative.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets the minimum stock level for the product.
    /// Used for automatic reorder alerts.
    /// </summary>
    public int MinStockLevel { get; set; }

    /// <summary>
    /// Gets the active status of the product.
    /// Indicates whether the product can be sold.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the product information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the unique identifier of the product.
    /// </summary>
    /// <returns>The product's ID as a string.</returns>
    string IProduct.Id => Id.ToString();

    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    /// <returns>The product name.</returns>
    string IProduct.Name => Name;

    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    /// <returns>The product price.</returns>
    decimal IProduct.Price => Price;

    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    /// <returns>The product description.</returns>
    string IProduct.Description => Description;

    /// <summary>
    /// Gets the category of the product.
    /// </summary>
    /// <returns>The product category.</returns>
    string IProduct.Category => Category;

    /// <summary>
    /// Gets the image URL of the product.
    /// </summary>
    /// <returns>The product image URL.</returns>
    string IProduct.Image => Image;

    /// <summary>
    /// Gets the stock quantity of the product.
    /// </summary>
    /// <returns>The product stock quantity.</returns>
    int IProduct.StockQuantity => StockQuantity;

    /// <summary>
    /// Gets the minimum stock level for the product.
    /// </summary>
    /// <returns>The minimum stock level.</returns>
    int IProduct.MinStockLevel => MinStockLevel;

    /// <summary>
    /// Gets the active status of the product.
    /// </summary>
    /// <returns>True if the product is active; otherwise, false.</returns>
    bool IProduct.IsActive => IsActive;

    /// <summary>
    /// Gets the creation date of the product.
    /// </summary>
    /// <returns>The product creation date.</returns>
    DateTime IProduct.CreatedAt => CreatedAt;

    /// <summary>
    /// Initializes a new instance of the Product class.
    /// </summary>
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        MinStockLevel = 10;
    }

    /// <summary>
    /// Creates a new product with the specified information.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="price">The product price.</param>
    /// <param name="description">The product description.</param>
    /// <param name="category">The product category.</param>
    /// <param name="stockQuantity">The initial stock quantity.</param>
    /// <param name="minStockLevel">The minimum stock level.</param>
    /// <param name="image">The product image URL.</param>
    /// <returns>A new Product instance.</returns>
    /// <exception cref="ArgumentException">Thrown when required parameters are null, empty, or invalid.</exception>
    public static Product Create(string name, decimal price, string description, string category,
        int stockQuantity = 0, int minStockLevel = 10, string? image = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be null or empty.", nameof(name));

        if (price <= 0)
            throw new ArgumentException("Product price must be greater than zero.", nameof(price));

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Product category cannot be null or empty.", nameof(category));

        if (stockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(stockQuantity));

        if (minStockLevel < 0)
            throw new ArgumentException("Minimum stock level cannot be negative.", nameof(minStockLevel));

        return new Product
        {
            Name = name.Trim(),
            Price = price,
            Description = description?.Trim() ?? string.Empty,
            Category = category.Trim(),
            StockQuantity = stockQuantity,
            MinStockLevel = minStockLevel,
            Image = image?.Trim() ?? string.Empty
        };
    }

    /// <summary>
    /// Activates the product.
    /// Changes the product's status to Active.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the product.
    /// Changes the product's status to Inactive.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the product price.
    /// </summary>
    /// <param name="newPrice">The new price for the product.</param>
    /// <exception cref="ArgumentException">Thrown when the new price is not greater than zero.</exception>
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Product price must be greater than zero.", nameof(newPrice));

        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds stock to the product inventory.
    /// </summary>
    /// <param name="quantity">The quantity to add to stock.</param>
    /// <exception cref="ArgumentException">Thrown when quantity is not positive.</exception>
    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to add must be positive.", nameof(quantity));

        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes stock from the product inventory.
    /// </summary>
    /// <param name="quantity">The quantity to remove from stock.</param>
    /// <exception cref="ArgumentException">Thrown when quantity is not positive or exceeds available stock.</exception>
    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to remove must be positive.", nameof(quantity));

        if (quantity > StockQuantity)
            throw new ArgumentException("Cannot remove more stock than available.", nameof(quantity));

        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the minimum stock level for the product.
    /// </summary>
    /// <param name="minLevel">The new minimum stock level.</param>
    /// <exception cref="ArgumentException">Thrown when minimum level is negative.</exception>
    public void UpdateMinStockLevel(int minLevel)
    {
        if (minLevel < 0)
            throw new ArgumentException("Minimum stock level cannot be negative.", nameof(minLevel));

        MinStockLevel = minLevel;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if the product stock is below the minimum level.
    /// </summary>
    /// <returns>True if stock is below minimum level; otherwise, false.</returns>
    public bool IsLowStock()
    {
        return StockQuantity <= MinStockLevel;
    }

    /// <summary>
    /// Checks if the product is out of stock.
    /// </summary>
    /// <returns>True if stock quantity is zero; otherwise, false.</returns>
    public bool IsOutOfStock()
    {
        return StockQuantity == 0;
    }

    /// <summary>
    /// Checks if the product is available for sale.
    /// </summary>
    /// <returns>True if product is active and has stock; otherwise, false.</returns>
    public bool IsAvailableForSale()
    {
        return IsActive && !IsOutOfStock();
    }

    /// <summary>
    /// Performs validation of the product entity using the ProductValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new ProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
