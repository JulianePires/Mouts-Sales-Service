using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// The generated users will have valid:
    /// - SaleNumber (generating code SALEXXXX pattern)
    /// - SaleDate (Date valid format)
    /// - CustomerId (generate id CUSTXXX pattern)
    /// - CustomerName (faker name)
    /// - BranchId (generate id BRANCHXXX pattern)
    /// - BranchName (faker city name)
    /// - TotalAmount (
    /// /// </summary>
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .RuleFor(s => s.SaleNumber, f => f.Random.Replace("SALE####"))
        .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
        .RuleFor(s => s.CustomerId, f => f.Random.Replace("CUST####"))
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => f.Random.Replace("BRANCH####"))
        .RuleFor(s => s.BranchName, f => f.Address.City())
        .RuleFor(s => s.TotalAmount, f => f.Finance.Amount());

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated sale will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateValidSale()
    {
        return SaleFaker.Generate();
    }
}