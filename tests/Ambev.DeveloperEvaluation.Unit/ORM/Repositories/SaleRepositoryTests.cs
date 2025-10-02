using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.ORM.Repositories;

/// <summary>
/// Contains unit tests for the SaleRepository class.
/// Tests cover CRUD operations, relationship loading, and query methods.
/// </summary>
public class SaleRepositoryTests : IDisposable
{
    private readonly DefaultContext _context;
    private readonly ISaleRepository _repository;

    public SaleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DefaultContext(options);
        _repository = new SaleRepository(_context);
    }

    /// <summary>
    /// Tests that CreateAsync successfully creates a new sale.
    /// </summary>
    [Fact(DisplayName = "Given valid sale When creating Then should return created sale with ID")]
    public async Task Given_ValidSale_When_Creating_Then_ShouldReturnCreatedSaleWithId()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var sale = SaleTestData.GenerateValidSale(customer, branch);

        // Act
        var result = await _repository.CreateAsync(sale);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.SaleNumber.Should().NotBeNullOrEmpty();
        result.CustomerId.Should().Be(customer.Id);
        result.BranchId.Should().Be(branch.Id);
    }

    /// <summary>
    /// Tests that CreateAsync throws an exception when null sale is provided.
    /// </summary>
    [Fact(DisplayName = "Given null sale When creating Then should throw ArgumentNullException")]
    public async Task Given_NullSale_When_Creating_Then_ShouldThrowArgumentNullException()
    {
        // Arrange
        Sale nullSale = null!;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _repository.CreateAsync(nullSale);
        });
    }

    /// <summary>
    /// Tests that GetByIdAsync retrieves the correct sale.
    /// </summary>
    [Fact(DisplayName = "Given existing sale ID When getting by ID Then should return correct sale")]
    public async Task Given_ExistingSaleId_When_GettingById_Then_ShouldReturnCorrectSale()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var sale = SaleTestData.GenerateValidSale(customer, branch);
        await _repository.CreateAsync(sale);

        // Act
        var result = await _repository.GetByIdAsync(sale.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(sale.Id);
        result.SaleNumber.Should().Be(sale.SaleNumber);
    }

    /// <summary>
    /// Tests that GetByIdAsync returns null for non-existent sale.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When getting by ID Then should return null")]
    public async Task Given_NonExistentSaleId_When_GettingById_Then_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that GetByIdWithRelationsAsync retrieves sale with related entities.
    /// </summary>
    [Fact(DisplayName = "Given existing sale When getting with relations Then should include customer, branch and items")]
    public async Task Given_ExistingSale_When_GettingWithRelations_Then_ShouldIncludeRelatedEntities()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        var product = ProductTestData.GenerateValidProduct();

        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var sale = SaleTestData.GenerateValidSale(customer, branch);
        sale.AddItem(product, 2);
        await _repository.CreateAsync(sale);

        // Act
        var result = await _repository.GetByIdWithRelationsAsync(sale.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Customer.Should().NotBeNull();
        result.Customer.Name.Should().Be(customer.Name);
        result.Branch.Should().NotBeNull();
        result.Branch.Name.Should().Be(branch.Name);
        result.Items.Should().HaveCount(1);
        result.Items.First().Product.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that GetBySaleNumberAsync retrieves sale by sale number.
    /// </summary>
    [Fact(DisplayName = "Given existing sale number When getting by sale number Then should return correct sale")]
    public async Task Given_ExistingSaleNumber_When_GettingBySaleNumber_Then_ShouldReturnCorrectSale()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var sale = Sale.Create(customer, branch, "SALE-12345");
        await _repository.CreateAsync(sale);

        // Act
        var result = await _repository.GetBySaleNumberAsync("SALE-12345");

        // Assert
        result.Should().NotBeNull();
        result!.SaleNumber.Should().Be("SALE-12345");
    }

    /// <summary>
    /// Tests that GetByCustomerIdAsync retrieves sales for specific customer.
    /// </summary>
    [Fact(DisplayName = "Given customer with sales When getting by customer ID Then should return customer sales")]
    public async Task Given_CustomerWithSales_When_GettingByCustomerId_Then_ShouldReturnCustomerSales()
    {
        // Arrange
        var customer1 = CustomerTestData.GenerateValidCustomer();
        var customer2 = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();

        await _context.Customers.AddRangeAsync(customer1, customer2);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var sale1 = SaleTestData.GenerateValidSale(customer1, branch);
        var sale2 = SaleTestData.GenerateValidSale(customer1, branch);
        var sale3 = SaleTestData.GenerateValidSale(customer2, branch);

        await _repository.CreateAsync(sale1);
        await _repository.CreateAsync(sale2);
        await _repository.CreateAsync(sale3);

        // Act
        var result = await _repository.GetByCustomerIdAsync(customer1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.CustomerId == customer1.Id);
    }

    /// <summary>
    /// Tests that GetByBranchIdAsync retrieves sales for specific branch.
    /// </summary>
    [Fact(DisplayName = "Given branch with sales When getting by branch ID Then should return branch sales")]
    public async Task Given_BranchWithSales_When_GettingByBranchId_Then_ShouldReturnBranchSales()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch1 = BranchTestData.GenerateValidBranch();
        var branch2 = BranchTestData.GenerateValidBranch();

        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddRangeAsync(branch1, branch2);
        await _context.SaveChangesAsync();

        var sale1 = SaleTestData.GenerateValidSale(customer, branch1);
        var sale2 = SaleTestData.GenerateValidSale(customer, branch1);
        var sale3 = SaleTestData.GenerateValidSale(customer, branch2);

        await _repository.CreateAsync(sale1);
        await _repository.CreateAsync(sale2);
        await _repository.CreateAsync(sale3);

        // Act
        var result = await _repository.GetByBranchIdAsync(branch1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.BranchId == branch1.Id);
    }

    /// <summary>
    /// Tests that GetByDateRangeAsync retrieves sales within date range.
    /// </summary>
    [Fact(DisplayName = "Given sales in different dates When getting by date range Then should return sales in range")]
    public async Task Given_SalesInDifferentDates_When_GettingByDateRange_Then_ShouldReturnSalesInRange()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var baseDate = DateTime.UtcNow;
        var sale1 = SaleTestData.GenerateValidSale(customer, branch);
        sale1.SaleDate = baseDate.AddDays(-2);
        var sale2 = SaleTestData.GenerateValidSale(customer, branch);
        sale2.SaleDate = baseDate;
        var sale3 = SaleTestData.GenerateValidSale(customer, branch);
        sale3.SaleDate = baseDate.AddDays(2);

        await _repository.CreateAsync(sale1);
        await _repository.CreateAsync(sale2);
        await _repository.CreateAsync(sale3);

        // Act
        var result = await _repository.GetByDateRangeAsync(baseDate.AddDays(-1), baseDate.AddDays(1));

        // Assert
        result.Should().HaveCount(1);
        result.First().Id.Should().Be(sale2.Id);
    }

    /// <summary>
    /// Tests that UpdateAsync successfully updates a sale.
    /// </summary>
    [Fact(DisplayName = "Given existing sale When updating Then should save changes")]
    public async Task Given_ExistingSale_When_Updating_Then_ShouldSaveChanges()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var sale = Sale.Create(customer, branch, "SAL" + Guid.NewGuid().ToString("N")[..8]);
        await _repository.CreateAsync(sale);

        sale.Cancel();

        // Act
        var result = await _repository.UpdateAsync(sale);

        // Assert
        result.IsCancelled.Should().BeTrue();

        var savedSale = await _repository.GetByIdAsync(sale.Id);
        savedSale!.IsCancelled.Should().BeTrue();
    }

    /// <summary>
    /// Tests that DeleteAsync successfully deletes a sale.
    /// </summary>
    [Fact(DisplayName = "Given existing sale When deleting Then should remove from database")]
    public async Task Given_ExistingSale_When_Deleting_Then_ShouldRemoveFromDatabase()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var sale = SaleTestData.GenerateValidSale(customer, branch);
        await _repository.CreateAsync(sale);

        // Act
        var result = await _repository.DeleteAsync(sale.Id);

        // Assert
        result.Should().BeTrue();

        var deletedSale = await _repository.GetByIdAsync(sale.Id);
        deletedSale.Should().BeNull();
    }

    /// <summary>
    /// Tests that DeleteAsync returns false for non-existent sale.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When deleting Then should return false")]
    public async Task Given_NonExistentSaleId_When_Deleting_Then_ShouldReturnFalse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.DeleteAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that GetCountAsync returns correct count of sales.
    /// </summary>
    [Fact(DisplayName = "Given multiple sales When getting count Then should return correct number")]
    public async Task Given_MultipleSales_When_GettingCount_Then_ShouldReturnCorrectNumber()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        var sale1 = SaleTestData.GenerateValidSale(customer, branch);
        var sale2 = SaleTestData.GenerateValidSale(customer, branch);
        var sale3 = SaleTestData.GenerateValidSale(customer, branch);

        await _repository.CreateAsync(sale1);
        await _repository.CreateAsync(sale2);
        await _repository.CreateAsync(sale3);

        // Act
        var result = await _repository.GetCountAsync();

        // Assert
        result.Should().Be(3);
    }

    /// <summary>
    /// Tests that GetPaginatedAsync returns correct page of sales.
    /// </summary>
    [Fact(DisplayName = "Given multiple sales When getting paginated Then should return correct page")]
    public async Task Given_MultipleSales_When_GettingPaginated_Then_ShouldReturnCorrectPage()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        // Create 5 sales
        for (int i = 0; i < 5; i++)
        {
            var sale = SaleTestData.GenerateValidSale(customer, branch);
            await _repository.CreateAsync(sale);
            await Task.Delay(10); // Ensure different creation times
        }

        // Act
        var result = await _repository.GetPaginatedAsync(pageNumber: 2, pageSize: 2);

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests that LoadRelatedDataAsync loads all navigation properties.
    /// </summary>
    [Fact(DisplayName = "Given sale without loaded relations When loading relations Then should populate navigation properties")]
    public async Task Given_SaleWithoutLoadedRelations_When_LoadingRelations_Then_ShouldPopulateNavigationProperties()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();
        var product = ProductTestData.GenerateValidProduct();

        await _context.Customers.AddAsync(customer);
        await _context.Branches.AddAsync(branch);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var sale = SaleTestData.GenerateValidSale(customer, branch);
        sale.AddItem(product, 1);
        await _repository.CreateAsync(sale);

        // Get sale without relations
        var saleWithoutRelations = await _repository.GetByIdAsync(sale.Id);

        // Act
        await _repository.LoadRelatedDataAsync(saleWithoutRelations!);

        // Assert
        saleWithoutRelations!.Customer.Should().NotBeNull();
        saleWithoutRelations.Branch.Should().NotBeNull();
        saleWithoutRelations.Items.Should().HaveCount(1);
        saleWithoutRelations.Items.First().Product.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that GetPaginatedAsync throws exception for invalid page number.
    /// </summary>
    [Fact(DisplayName = "Given invalid page number When getting paginated sales Then should throw ArgumentOutOfRangeException")]
    public async Task Given_InvalidPageNumber_When_GettingPaginatedSales_Then_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await _repository.GetPaginatedAsync(0, 10);
        });

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await _repository.GetPaginatedAsync(-1, 10);
        });
    }

    /// <summary>
    /// Tests that GetPaginatedAsync throws exception for invalid page size.
    /// </summary>
    [Fact(DisplayName = "Given invalid page size When getting paginated sales Then should throw ArgumentOutOfRangeException")]
    public async Task Given_InvalidPageSize_When_GettingPaginatedSales_Then_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await _repository.GetPaginatedAsync(1, 0);
        });

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await _repository.GetPaginatedAsync(1, -5);
        });
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}