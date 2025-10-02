using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Customer entities using the Bogus library.
/// This class centralizes all customer test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CustomerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Customer entities.
    /// The generated customers will have valid:
    /// - Name (person names)
    /// - Email (valid format)
    /// - Phone (international format)
    /// - Address (full addresses)
    /// - BirthDate (18-80 years old)
    /// - IsActive (default true)
    /// </summary>
    private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
        .RuleFor(c => c.Id, _ => Guid.NewGuid()) // Ensure unique ID for each customer
        .RuleFor(c => c.Name, f => f.Person.FullName)
        .RuleFor(c => c.Email, f => f.Internet.Email().ToLowerInvariant())
        .RuleFor(c => c.Phone, f => "+55" + f.Random.String2(10, "123456789"))
        .RuleFor(c => c.Address, f => f.Address.FullAddress())
        .RuleFor(c => c.BirthDate, f => f.Date.Between(DateTime.Today.AddYears(-80), DateTime.Today.AddYears(-18)))
        .RuleFor(c => c.IsActive, _ => true)
        .RuleFor(c => c.CreatedAt, f => f.Date.Past(1));

    /// <summary>
    /// Generates a valid Customer entity with randomized data.
    /// </summary>
    /// <returns>A valid Customer entity with randomly generated data.</returns>
    public static Customer GenerateValidCustomer()
    {
        return CustomerFaker.Generate();
    }

    /// <summary>
    /// Generates multiple valid Customer entities.
    /// </summary>
    /// <param name="count">Number of customers to generate.</param>
    /// <returns>A list of valid Customer entities.</returns>
    public static List<Customer> GenerateValidCustomers(int count)
    {
        return CustomerFaker.Generate(count);
    }

    /// <summary>
    /// Generates a Customer with invalid name (empty).
    /// </summary>
    /// <returns>A Customer with validation errors.</returns>
    public static Customer GenerateCustomerWithInvalidName()
    {
        var customer = GenerateValidCustomer();
        customer.Name = string.Empty;
        return customer;
    }

    /// <summary>
    /// Generates a Customer with invalid email format.
    /// </summary>
    /// <returns>A Customer with validation errors.</returns>
    public static Customer GenerateCustomerWithInvalidEmail()
    {
        var customer = GenerateValidCustomer();
        customer.Email = "invalid-email";
        return customer;
    }

    /// <summary>
    /// Generates a Customer with empty email.
    /// </summary>
    /// <returns>A Customer with validation errors.</returns>
    public static Customer GenerateCustomerWithEmptyEmail()
    {
        var customer = GenerateValidCustomer();
        customer.Email = string.Empty;
        return customer;
    }

    /// <summary>
    /// Generates a Customer with future birth date.
    /// </summary>
    /// <returns>A Customer with validation errors.</returns>
    public static Customer GenerateCustomerWithFutureBirthDate()
    {
        var customer = GenerateValidCustomer();
        customer.BirthDate = DateTime.Today.AddDays(1);
        return customer;
    }

    /// <summary>
    /// Generates a Customer with birth date too far in the past.
    /// </summary>
    /// <returns>A Customer with validation errors.</returns>
    public static Customer GenerateCustomerWithTooOldBirthDate()
    {
        var customer = GenerateValidCustomer();
        customer.BirthDate = DateTime.Today.AddYears(-121);
        return customer;
    }

    /// <summary>
    /// Generates a Customer under 18 years old.
    /// </summary>
    /// <returns>A Customer under legal age.</returns>
    public static Customer GenerateUnderageCustomer()
    {
        var customer = GenerateValidCustomer();
        customer.BirthDate = DateTime.Today.AddYears(-17);
        return customer;
    }

    /// <summary>
    /// Generates an inactive Customer.
    /// </summary>
    /// <returns>An inactive Customer entity.</returns>
    public static Customer GenerateInactiveCustomer()
    {
        var customer = GenerateValidCustomer();
        customer.IsActive = false;
        return customer;
    }

    /// <summary>
    /// Generates a Customer without birth date.
    /// </summary>
    /// <returns>A Customer with null birth date.</returns>
    public static Customer GenerateCustomerWithoutBirthDate()
    {
        var customer = GenerateValidCustomer();
        customer.BirthDate = null;
        return customer;
    }

    /// <summary>
    /// Generates a valid email address using Faker.
    /// </summary>
    /// <returns>A valid email address.</returns>
    public static string GenerateValidEmail()
    {
        return new Faker().Internet.Email().ToLowerInvariant();
    }
}
