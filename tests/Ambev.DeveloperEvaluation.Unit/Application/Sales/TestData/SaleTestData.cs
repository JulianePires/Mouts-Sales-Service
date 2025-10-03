using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
/// Provides test data for Sale-related unit tests
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Generates a valid CreateSaleCommand for testing
    /// </summary>
    /// <returns>A valid CreateSaleCommand</returns>
    public static CreateSaleCommand GenerateValidCreateSaleCommand()
    {
        return new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            Items = new List<CreateSaleItemRequest>
            {
                new CreateSaleItemRequest
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 5
                },
                new CreateSaleItemRequest
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 3
                }
            }
        };
    }

    /// <summary>
    /// Generates a CreateSaleCommand with invalid data for testing validation
    /// </summary>
    /// <returns>An invalid CreateSaleCommand</returns>
    public static CreateSaleCommand GenerateInvalidCreateSaleCommand()
    {
        return new CreateSaleCommand
        {
            CustomerId = Guid.Empty, // Invalid
            BranchId = Guid.Empty,   // Invalid
            Items = new List<CreateSaleItemRequest>() // Empty items - Invalid
        };
    }

    /// <summary>
    /// Generates a valid Sale entity for testing
    /// </summary>
    /// <returns>A valid Sale entity</returns>
    public static Sale GenerateValidSale()
    {
        // Use Common TestData builders for entity creation
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();

        product1.StockQuantity = 100;
        product2.StockQuantity = 50;

        var sale = Sale.Create(customer, branch, "SAL202410021234", DateTime.UtcNow);

        // Add items to the sale using the proper domain method
        sale.AddItem(product1, 5);
        sale.AddItem(product2, 3);

        return sale;
    }

    /// <summary>
    /// Generates a cancelled Sale entity for testing
    /// </summary>
    /// <returns>A cancelled Sale entity</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateValidSale();
        sale.IsCancelled = true;
        sale.UpdatedAt = DateTime.UtcNow;

        foreach (var item in sale.Items)
        {
            item.IsCancelled = true;
        }

        return sale;
    }

    /// <summary>
    /// Generates a list of sales for pagination testing
    /// </summary>
    /// <param name="count">Number of sales to generate</param>
    /// <returns>A list of Sale entities</returns>
    public static List<Sale> GenerateSalesList(int count)
    {
        var sales = new List<Sale>();

        for (int i = 0; i < count; i++)
        {
            var sale = GenerateValidSale();
            sale.SaleNumber = $"SAL{DateTime.UtcNow:yyyyMMdd}{i:D4}";
            sale.SaleDate = DateTime.UtcNow.AddDays(-i);
            sales.Add(sale);
        }

        return sales;
    }
}