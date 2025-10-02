using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

// Temporarily commented out due to ambiguous call issue
// using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.ORM.Repositories;

/// <summary>
/// Contains unit tests for the <see cref="CustomerRepository"/> class.
/// Tests cover CRUD operations, filtering, searching, and specific business logic methods.
/// </summary>
public class CustomerRepositoryTests : IDisposable
{
    private readonly DefaultContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DefaultContext(options);
        _repository = new CustomerRepository(_context);
    }

    /// <summary>
    /// Tests that CreateAsync successfully creates a customer when valid data is provided.
    /// </summary>
    [Fact(DisplayName = "Given valid customer When creating Then should return created customer")]
    public async Task CreateAsync_WithValidCustomer_ShouldReturnCreatedCustomer()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        // Act
        var result = await _repository.CreateAsync(customer);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(customer.Name);
        result.Email.Should().Be(customer.Email);
    }

    /// <summary>
    /// Tests that CreateAsync throws an exception when null customer is provided.
    /// </summary>
    [Fact(DisplayName = "Given null customer When creating Then should throw ArgumentNullException")]
    public async Task CreateAsync_WithNullCustomer_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await FluentActions.Invoking(() => _repository.CreateAsync(null!))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that GetByIdAsync returns customer when valid ID is provided.
    /// </summary>
    [Fact(DisplayName = "Given existing customer ID When getting by ID Then should return customer")]
    public async Task GetByIdAsync_WithValidId_ShouldReturnCustomer()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();

        var createdCustomer = await _repository.CreateAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(createdCustomer.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(createdCustomer.Id);
        result.Name.Should().Be(customer.Name);
    }

    /// <summary>
    /// Tests that GetByIdAsync returns null when customer ID does not exist.
    /// </summary>
    [Fact(DisplayName = "Given non-existing customer ID When getting by ID Then should return null")]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that GetByEmailAsync returns customer when valid email is provided.
    /// </summary>
    [Fact(DisplayName = "Given existing customer email When getting by email Then should return customer")]
    public async Task GetByEmailAsync_WithValidEmail_ShouldReturnCustomer()
    {
        // Arrange
        var email = "test@example.com";
        var customer = new Customer
        {
            Name = "Test Customer",
            Email = email,
            Phone = "(11) 77777-7777",
            BirthDate = DateTime.Now.AddYears(-30),
            IsActive = true
        };

        await _repository.CreateAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(email);
    }

    /// <summary>
    /// Tests that GetByPhoneAsync returns customer when valid phone is provided.
    /// </summary>
    [Fact(DisplayName = "Given existing customer phone When getting by phone Then should return customer")]
    public async Task GetByPhoneAsync_WithValidPhone_ShouldReturnCustomer()
    {
        // Arrange
        var phone = "(11) 66666-6666";
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Phone = phone;
        await _repository.CreateAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByPhoneAsync(phone);

        // Assert
        result.Should().NotBeNull();
        result!.Phone.Should().Be(phone);
    }

    /// <summary>
    /// Tests that GetByStatusAsync returns customers with specified status.
    /// </summary>
    [Fact(DisplayName = "Given customers with different status When getting by status Then should return filtered customers")]
    public async Task GetByStatusAsync_WithValidStatus_ShouldReturnFilteredCustomers()
    {
        // Arrange
        var activeCustomer = CustomerTestData.GenerateValidCustomer();
        activeCustomer.IsActive = true;
        var inactiveCustomer = CustomerTestData.GenerateValidCustomer();
        inactiveCustomer.IsActive = false;

        await _repository.CreateAsync(activeCustomer);
        await _repository.CreateAsync(inactiveCustomer);
        await _context.SaveChangesAsync();

        // Act
        var activeResults = await _repository.GetByStatusAsync(true);

        // Assert
        activeResults.Should().HaveCount(1);
        activeResults.First().IsActive.Should().BeTrue();
    }

    /// <summary>
    /// Tests that SearchByNameAsync returns customers matching search term.
    /// </summary>
    [Fact(DisplayName = "Given customers with various names When searching by name Then should return matching customers")]
    public async Task SearchByNameAsync_WithSearchTerm_ShouldReturnMatchingCustomers()
    {
        // Arrange
        var customer1 = CustomerTestData.GenerateValidCustomer();
        customer1.Name = "John Smith";
        var customer2 = CustomerTestData.GenerateValidCustomer();
        customer2.Name = "John Doe";
        var customer3 = CustomerTestData.GenerateValidCustomer();
        customer3.Name = "Jane Smith";

        await _repository.CreateAsync(customer1);
        await _repository.CreateAsync(customer2);
        await _repository.CreateAsync(customer3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.SearchByNameAsync("John");

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(c => c.Name.Contains("John"));
    }

    /// <summary>
    /// Tests that GetByAgeRangeAsync returns customers within specified age range.
    /// </summary>
    [Fact(DisplayName = "Given customers with various ages When getting by age range Then should return customers in range")]
    public async Task GetByAgeRangeAsync_WithValidRange_ShouldReturnCustomersInRange()
    {
        // Arrange
        var customer1 = CustomerTestData.GenerateValidCustomer();
        var customer2 = CustomerTestData.GenerateValidCustomer();
        customer2.Name = "Middle Customer";
        var customer3 = CustomerTestData.GenerateValidCustomer();
        customer3.Name = "Old Customer";
        customer1.BirthDate = DateTime.Now.AddYears(-20); // Age 20
        customer2.BirthDate = DateTime.Now.AddYears(-35); // Age 30
        customer3.BirthDate = DateTime.Now.AddYears(-60); // Age 60

        await _repository.CreateAsync(customer1);
        await _repository.CreateAsync(customer2);
        await _repository.CreateAsync(customer3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetByAgeRangeAsync(25, 35);

        // Assert
        results.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests that UpdateAsync successfully updates customer information.
    /// </summary>
    [Fact(DisplayName = "Given valid customer updates When updating Then should save changes")]
    public async Task UpdateAsync_WithValidCustomer_ShouldUpdateCustomer()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.BirthDate = DateTime.Now.AddYears(-26);

        var createdCustomer = await _repository.CreateAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        createdCustomer.Name = "Updated Name";
        createdCustomer.Email = "updated@example.com";
        await _repository.UpdateAsync(createdCustomer);
        await _context.SaveChangesAsync();

        // Assert
        var updatedCustomer = await _repository.GetByIdAsync(createdCustomer.Id);
        updatedCustomer!.Name.Should().Be("Updated Name");
        updatedCustomer.Email.Should().Be("updated@example.com");
    }

    /// <summary>
    /// Tests that DeleteAsync successfully removes customer.
    /// </summary>
    [Fact(DisplayName = "Given existing customer When deleting Then should remove customer")]
    public async Task DeleteAsync_WithExistingCustomer_ShouldRemoveCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Customer To Delete",
            Email = "delete@example.com",
            BirthDate = DateTime.Now.AddYears(-45),
            IsActive = true
        };

        var createdCustomer = await _repository.CreateAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(createdCustomer.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedCustomer = await _repository.GetByIdAsync(createdCustomer.Id);
        deletedCustomer.Should().BeNull();
    }



    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}