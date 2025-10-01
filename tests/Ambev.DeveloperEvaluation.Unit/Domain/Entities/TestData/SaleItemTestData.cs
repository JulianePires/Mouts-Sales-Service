using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for SaleItem entities using the Bogus library.
/// This class centralizes all sale item test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleItemTestData
{
    /// <summary>
    /// Generates a valid SaleItem entity with randomized data.
    /// Uses valid products and reasonable quantities.
    /// </summary>
    /// <param name="saleId">Optional sale ID to associate with the item.</param>
    /// <param name="product">Optional specific product to use.</param>
    /// <returns>A valid SaleItem entity with randomly generated data.</returns>
    public static SaleItem GenerateValidSaleItem(Guid? saleId = null, Product? product = null)
    {
        var faker = new Faker();
        var actualSaleId = saleId ?? Guid.NewGuid();
        var actualProduct = product ?? ProductTestData.GenerateValidProduct();
        var quantity = faker.Random.Int(1, 20);
        
        return SaleItem.Create(actualSaleId, actualProduct, quantity);
    }

    /// <summary>
    /// Generates multiple valid SaleItem entities.
    /// </summary>
    /// <param name="count">Number of sale items to generate.</param>
    /// <param name="saleId">Optional sale ID to associate with all items.</param>
    /// <returns>A list of valid SaleItem entities.</returns>
    public static List<SaleItem> GenerateValidSaleItems(int count, Guid? saleId = null)
    {
        var actualSaleId = saleId ?? Guid.NewGuid();
        var items = new List<SaleItem>();
        
        for (int i = 0; i < count; i++)
        {
            items.Add(GenerateValidSaleItem(actualSaleId));
        }
        
        return items;
    }

    /// <summary>
    /// Generates a SaleItem with quantity that triggers 10% discount (4-9 items).
    /// </summary>
    /// <param name="saleId">Optional sale ID to associate with the item.</param>
    /// <returns>A SaleItem with 10% discount.</returns>
    public static SaleItem GenerateSaleItemWith10PercentDiscount(Guid? saleId = null)
    {
        var faker = new Faker();
        var actualSaleId = saleId ?? Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        var quantity = faker.Random.Int(4, 9);
        
        return SaleItem.Create(actualSaleId, product, quantity);
    }

    /// <summary>
    /// Generates a SaleItem with quantity that triggers 20% discount (10-20 items).
    /// </summary>
    /// <param name="saleId">Optional sale ID to associate with the item.</param>
    /// <returns>A SaleItem with 20% discount.</returns>
    public static SaleItem GenerateSaleItemWith20PercentDiscount(Guid? saleId = null)
    {
        var faker = new Faker();
        var actualSaleId = saleId ?? Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        var quantity = faker.Random.Int(10, 20);
        
        return SaleItem.Create(actualSaleId, product, quantity);
    }

    /// <summary>
    /// Generates a SaleItem with quantity that has no discount (1-3 items).
    /// </summary>
    /// <param name="saleId">Optional sale ID to associate with the item.</param>
    /// <returns>A SaleItem with no discount.</returns>
    public static SaleItem GenerateSaleItemWithNoDiscount(Guid? saleId = null)
    {
        var faker = new Faker();
        var actualSaleId = saleId ?? Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        var quantity = faker.Random.Int(1, 3);
        
        return SaleItem.Create(actualSaleId, product, quantity);
    }

    /// <summary>
    /// Generates a SaleItem with maximum allowed quantity (20 items).
    /// </summary>
    /// <param name="saleId">Optional sale ID to associate with the item.</param>
    /// <returns>A SaleItem with maximum quantity.</returns>
    public static SaleItem GenerateSaleItemWithMaxQuantity(Guid? saleId = null)
    {
        var actualSaleId = saleId ?? Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        
        return SaleItem.Create(actualSaleId, product, 20);
    }

    /// <summary>
    /// Generates a cancelled SaleItem.
    /// </summary>
    /// <param name="saleId">Optional sale ID to associate with the item.</param>
    /// <returns>A cancelled SaleItem entity.</returns>
    public static SaleItem GenerateCancelledSaleItem(Guid? saleId = null)
    {
        var item = GenerateValidSaleItem(saleId);
        item.Cancel();
        return item;
    }

    /// <summary>
    /// Generates SaleItems that together exceed the 20-unit limit for a single product.
    /// </summary>
    /// <param name="saleId">The sale ID to associate with all items.</param>
    /// <returns>A list of SaleItems that violate the business rule.</returns>
    public static List<SaleItem> GenerateSaleItemsExceedingProductLimit(Guid saleId)
    {
        var product = ProductTestData.GenerateValidProduct();
        var items = new List<SaleItem>
        {
            SaleItem.Create(saleId, product, 12),
            SaleItem.Create(saleId, product, 10) // Total: 22 units (exceeds limit)
        };
        
        return items;
    }

    /// <summary>
    /// Creates a SaleItem with custom unit price (different from product price).
    /// </summary>
    /// <param name="unitPrice">The custom unit price.</param>
    /// <param name="saleId">Optional sale ID to associate with the item.</param>
    /// <returns>A SaleItem with custom pricing.</returns>
    public static SaleItem GenerateSaleItemWithCustomPrice(decimal unitPrice, Guid? saleId = null)
    {
        var actualSaleId = saleId ?? Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        var faker = new Faker();
        var quantity = faker.Random.Int(1, 10);
        
        return SaleItem.Create(actualSaleId, product, quantity, unitPrice);
    }
}
