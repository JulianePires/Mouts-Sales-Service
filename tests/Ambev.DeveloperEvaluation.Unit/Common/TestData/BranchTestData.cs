using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Branch entities using the Bogus library.
/// This class centralizes all branch test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class BranchTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Branch entities.
    /// The generated branches will have valid:
    /// - Name (company names)
    /// - Address (full addresses)
    /// - Phone (international format)
    /// - Email (valid business emails)
    /// - Manager (person names)
    /// - IsActive (default true)
    /// </summary>
    private static readonly Faker<Branch> BranchFaker = new Faker<Branch>()
        .RuleFor(b => b.Id, _ => Guid.NewGuid()) // Ensure unique ID for each branch
        .RuleFor(b => b.Name, f => f.Company.CompanyName())
        .RuleFor(b => b.Address, f => f.Address.FullAddress())
        .RuleFor(b => b.Phone, f => "+55" + f.Random.String2(10, "123456789"))
        .RuleFor(b => b.Email, f => f.Internet.Email())
        .RuleFor(b => b.Manager, f => f.Person.FullName)
        .RuleFor(b => b.IsActive, _ => true)
        .RuleFor(b => b.CreatedAt, f => f.Date.Past(1));

    /// <summary>
    /// Generates a valid Branch entity with randomized data.
    /// </summary>
    /// <returns>A valid Branch entity with randomly generated data.</returns>
    public static Branch GenerateValidBranch()
    {
        return BranchFaker.Generate();
    }

    /// <summary>
    /// Generates multiple valid Branch entities.
    /// </summary>
    /// <param name="count">Number of branches to generate.</param>
    /// <returns>A list of valid Branch entities.</returns>
    public static List<Branch> GenerateValidBranches(int count)
    {
        return BranchFaker.Generate(count);
    }

    /// <summary>
    /// Generates a Branch with invalid name (empty).
    /// </summary>
    /// <returns>A Branch with validation errors.</returns>
    public static Branch GenerateBranchWithInvalidName()
    {
        var branch = GenerateValidBranch();
        branch.Name = string.Empty;
        return branch;
    }

    /// <summary>
    /// Generates a Branch with invalid address (too short).
    /// </summary>
    /// <returns>A Branch with validation errors.</returns>
    public static Branch GenerateBranchWithInvalidAddress()
    {
        var branch = GenerateValidBranch();
        branch.Address = "123";
        return branch;
    }

    /// <summary>
    /// Generates a Branch with invalid email format.
    /// </summary>
    /// <returns>A Branch with validation errors.</returns>
    public static Branch GenerateBranchWithInvalidEmail()
    {
        var branch = GenerateValidBranch();
        branch.Email = "invalid-email";
        return branch;
    }

    /// <summary>
    /// Generates a Branch with future creation date.
    /// </summary>
    /// <returns>A Branch with validation errors.</returns>
    public static Branch GenerateBranchWithFutureCreationDate()
    {
        var branch = GenerateValidBranch();
        branch.CreatedAt = DateTime.UtcNow.AddDays(1);
        return branch;
    }

    /// <summary>
    /// Generates an inactive Branch.
    /// </summary>
    /// <returns>An inactive Branch entity.</returns>
    public static Branch GenerateInactiveBranch()
    {
        var branch = GenerateValidBranch();
        branch.IsActive = false;
        return branch;
    }
}
