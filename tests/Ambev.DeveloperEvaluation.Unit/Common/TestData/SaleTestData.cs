using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Sale entities using the Bogus library.
/// This class centralizes all sale test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated sale will have valid customer, branch, and sale number.
    /// </summary>
    /// <param name="customer">Optional specific customer to use.</param>
    /// <param name="branch">Optional specific branch to use.</param>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateValidSale(Customer? customer = null, Branch? branch = null)
    {
        var actualCustomer = customer ?? CustomerTestData.GenerateValidCustomer();
        var actualBranch = branch ?? BranchTestData.GenerateValidBranch();

        return Sale.Create(actualCustomer, actualBranch, "SAL202410021234");
    }

    /// <summary>
    /// Generates multiple valid Sale entities.
    /// </summary>
    /// <param name="count">Number of sales to generate.</param>
    /// <returns>A list of valid Sale entities.</returns>
    public static List<Sale> GenerateValidSales(int count)
    {
        var sales = new List<Sale>();
        for (int i = 0; i < count; i++)
        {
            sales.Add(GenerateValidSale());
        }
        return sales;
    }

    /// <summary>
    /// Generates a Sale with items that have various discount levels.
    /// </summary>
    /// <returns>A Sale with mixed discount items.</returns>
    public static Sale GenerateSaleWithMixedDiscounts()
    {
        var sale = GenerateValidSale();

        // Add items with different discount levels
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();
        var product3 = ProductTestData.GenerateValidProduct();

        sale.AddItem(product1, 2);  // No discount
        sale.AddItem(product2, 6);  // 10% discount
        sale.AddItem(product3, 15); // 20% discount

        return sale;
    }

    /// <summary>
    /// Generates a Sale with items that reach the maximum quantity limit.
    /// </summary>
    /// <returns>A Sale with maximum quantity items.</returns>
    public static Sale GenerateSaleWithMaxQuantityItems()
    {
        var sale = GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();

        sale.AddItem(product, 20); // Maximum allowed quantity

        return sale;
    }

    /// <summary>
    /// Generates a cancelled Sale.
    /// </summary>
    /// <returns>A cancelled Sale entity.</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateValidSale();
        sale.Cancel();
        return sale;
    }

    /// <summary>
    /// Generates a Sale with inactive customer.
    /// </summary>
    /// <returns>A Sale with validation issues.</returns>
    public static Sale GenerateSaleWithInactiveCustomer()
    {
        var inactiveCustomer = CustomerTestData.GenerateInactiveCustomer();
        var branch = BranchTestData.GenerateValidBranch();

        return Sale.Create(inactiveCustomer, branch, "SAL202410021235");
    }

    /// <summary>
    /// Generates a Sale with inactive branch.
    /// </summary>
    /// <returns>A Sale with validation issues.</returns>
    public static Sale GenerateSaleWithInactiveBranch()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        var inactiveBranch = BranchTestData.GenerateInactiveBranch();

        return Sale.Create(customer, inactiveBranch, "SAL202410021236");
    }

    /// <summary>
    /// Generates a Sale with empty sale number.
    /// </summary>
    /// <returns>A Sale with validation errors.</returns>
    public static Sale GenerateSaleWithEmptyNumber()
    {
        var sale = GenerateValidSale();
        sale.SaleNumber = string.Empty;
        return sale;
    }

    /// <summary>
    /// Generates a Sale with future sale date.
    /// </summary>
    /// <returns>A Sale with validation errors.</returns>
    public static Sale GenerateSaleWithFutureDate()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        var futureDate = DateTime.UtcNow.AddDays(2);

        return Sale.Create(customer, branch, "SAL202410021237", futureDate);
    }
}