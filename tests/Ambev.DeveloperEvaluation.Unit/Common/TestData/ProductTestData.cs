using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Product entities using the Bogus library.
/// This class centralizes all product test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ProductTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// The generated products will have valid:
    /// - Name (product names)
    /// - Price (positive values)
    /// - Description (commerce descriptions)
    /// - Category (product categories)
    /// - StockQuantity (non-negative values)
    /// - MinStockLevel (non-negative values)
    /// - IsActive (default true)
    /// </summary>
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(p => p.Id, _ => Guid.NewGuid()) // Ensure unique ID for each product
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 100))
        .RuleFor(p => p.MinStockLevel, f => f.Random.Int(5, 15))
        .RuleFor(p => p.IsActive, _ => true)
        .RuleFor(p => p.CreatedAt, f => f.Date.Past(1));

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates multiple valid Product entities.
    /// </summary>
    /// <param name="count">Number of products to generate.</param>
    /// <returns>A list of valid Product entities.</returns>
    public static List<Product> GenerateValidProducts(int count)
    {
        return ProductFaker.Generate(count);
    }

    /// <summary>
    /// Generates a Product with invalid name (empty).
    /// </summary>
    /// <returns>A Product with validation errors.</returns>
    public static Product GenerateProductWithInvalidName()
    {
        var product = GenerateValidProduct();
        product.Name = string.Empty;
        return product;
    }

    /// <summary>
    /// Generates a Product with invalid price (zero or negative).
    /// </summary>
    /// <returns>A Product with validation errors.</returns>
    public static Product GenerateProductWithInvalidPrice()
    {
        var product = GenerateValidProduct();
        product.Price = 0;
        return product;
    }

    /// <summary>
    /// Generates a Product with negative price.
    /// </summary>
    /// <returns>A Product with validation errors.</returns>
    public static Product GenerateProductWithNegativePrice()
    {
        var product = GenerateValidProduct();
        product.Price = -10.50m;
        return product;
    }

    /// <summary>
    /// Generates a Product with invalid category (empty).
    /// </summary>
    /// <returns>A Product with validation errors.</returns>
    public static Product GenerateProductWithInvalidCategory()
    {
        var product = GenerateValidProduct();
        product.Category = string.Empty;
        return product;
    }

    /// <summary>
    /// Generates a Product with negative stock quantity.
    /// </summary>
    /// <returns>A Product with validation errors.</returns>
    public static Product GenerateProductWithNegativeStock()
    {
        var product = GenerateValidProduct();
        product.StockQuantity = -5;
        return product;
    }

    /// <summary>
    /// Generates a Product with negative minimum stock level.
    /// </summary>
    /// <returns>A Product with validation errors.</returns>
    public static Product GenerateProductWithNegativeMinStock()
    {
        var product = GenerateValidProduct();
        product.MinStockLevel = -3;
        return product;
    }

    /// <summary>
    /// Generates an out-of-stock Product.
    /// </summary>
    /// <returns>A Product with zero stock.</returns>
    public static Product GenerateOutOfStockProduct()
    {
        var product = GenerateValidProduct();
        product.StockQuantity = 0;
        return product;
    }

    /// <summary>
    /// Generates a low-stock Product.
    /// </summary>
    /// <returns>A Product with stock below minimum level.</returns>
    public static Product GenerateLowStockProduct()
    {
        var product = GenerateValidProduct();
        product.MinStockLevel = 10;
        product.StockQuantity = 5;
        return product;
    }

    /// <summary>
    /// Generates an inactive Product.
    /// </summary>
    /// <returns>An inactive Product entity.</returns>
    public static Product GenerateInactiveProduct()
    {
        var product = GenerateValidProduct();
        product.IsActive = false;
        return product;
    }

    /// <summary>
    /// Generates a Product that is not available for sale (inactive or out of stock).
    /// </summary>
    /// <returns>A Product not available for sale.</returns>
    public static Product GenerateUnavailableProduct()
    {
        var product = GenerateValidProduct();
        var random = new Random();
        if (random.Next(0, 2) == 0)
        {
            product.IsActive = false;
        }
        else
        {
            product.StockQuantity = 0;
        }
        return product;
    }
}
