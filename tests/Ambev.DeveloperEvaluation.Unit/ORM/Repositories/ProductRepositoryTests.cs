using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.ORM.Repositories;

/// <summary>
/// Contains unit tests for the <see cref="ProductRepository"/> class.
/// Tests cover CRUD operations, filtering, searching, and specific business logic methods.
/// </summary>
public class ProductRepositoryTests : IDisposable
{
    private readonly DefaultContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DefaultContext(options);
        _repository = new ProductRepository(_context);
    }

    /// <summary>
    /// Tests that CreateAsync successfully creates a product when valid data is provided.
    /// </summary>
    [Fact(DisplayName = "Given valid product When creating Then should return created product")]
    public async Task CreateAsync_WithValidProduct_ShouldReturnCreatedProduct()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act
        var result = await _repository.CreateAsync(product);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(product.Name);
        result.Price.Should().Be(product.Price);
        result.StockQuantity.Should().Be(product.StockQuantity);
    }

    /// <summary>
    /// Tests that CreateAsync throws an exception when null product is provided.
    /// </summary>
    [Fact(DisplayName = "Given null product When creating Then should throw ArgumentNullException")]
    public async Task CreateAsync_WithNullProduct_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await FluentActions.Invoking(() => _repository.CreateAsync(null!))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that GetByIdAsync returns product when valid ID is provided.
    /// </summary>
    [Fact(DisplayName = "Given existing product ID When getting by ID Then should return product")]
    public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        var createdProduct = await _repository.CreateAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(createdProduct.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(createdProduct.Id);
        result.Name.Should().Be(product.Name);
    }

    /// <summary>
    /// Tests that GetByIdAsync returns null when product ID does not exist.
    /// </summary>
    [Fact(DisplayName = "Given non-existing product ID When getting by ID Then should return null")]
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
    /// Tests that GetByCategoryAsync returns products from specified category.
    /// </summary>
    [Fact(DisplayName = "Given products in category When getting by category Then should return filtered products")]
    public async Task GetByCategoryAsync_WithValidCategory_ShouldReturnCategoryProducts()
    {
        // Arrange
        var category = "Electronics";
        var product1 = ProductTestData.GenerateValidProduct();
        product1.Category = category;
        var product2 = ProductTestData.GenerateValidProduct();
        product2.Category = category;
        var product3 = ProductTestData.GenerateValidProduct();
        product3.Category = "Books";

        await _repository.CreateAsync(product1);
        await _repository.CreateAsync(product2);
        await _repository.CreateAsync(product3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetByCategoryAsync(category);

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(p => p.Category == category);
    }

    /// <summary>
    /// Tests that GetInStockAsync returns only products with available stock.
    /// </summary>
    [Fact(DisplayName = "Given products with various stock levels When getting in stock Then should return only available products")]
    public async Task GetInStockAsync_ShouldReturnOnlyAvailableProducts()
    {
        // Arrange
        var product1 = ProductTestData.GenerateValidProduct();
        product1.StockQuantity = 10;
        var product2 = ProductTestData.GenerateOutOfStockProduct();
        var product3 = ProductTestData.GenerateValidProduct();
        product3.StockQuantity = 5;

        await _repository.CreateAsync(product1);
        await _repository.CreateAsync(product2);
        await _repository.CreateAsync(product3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetInStockAsync();

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(p => p.StockQuantity > 0);
    }

    /// <summary>
    /// Tests that GetLowStockAsync returns products with stock below minimum level.
    /// </summary>
    [Fact(DisplayName = "Given products with various stock levels When getting low stock Then should return products below minimum")]
    public async Task GetLowStockAsync_ShouldReturnProductsBelowMinimum()
    {
        // Arrange
        var product1 = ProductTestData.GenerateValidProduct();
        product1.StockQuantity = 5;
        product1.MinStockLevel = 10;
        var product2 = ProductTestData.GenerateValidProduct();
        product2.StockQuantity = 15;
        product2.MinStockLevel = 10;
        var product3 = ProductTestData.GenerateLowStockProduct();

        await _repository.CreateAsync(product1);
        await _repository.CreateAsync(product2);
        await _repository.CreateAsync(product3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetLowStockAsync(10);

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(p => p.StockQuantity <= p.MinStockLevel);
    }

    /// <summary>
    /// Tests that IsAvailableAsync returns true for products with sufficient stock.
    /// </summary>
    [Fact(DisplayName = "Given product with sufficient stock When checking availability Then should return true")]
    public async Task IsAvailableAsync_WithSufficientStock_ShouldReturnTrue()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 10;
        product.IsActive = true;
        var createdProduct = await _repository.CreateAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.IsAvailableAsync(createdProduct.Id, 5);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that IsAvailableAsync returns false for products with insufficient stock.
    /// </summary>
    [Fact(DisplayName = "Given product with insufficient stock When checking availability Then should return false")]
    public async Task IsAvailableAsync_WithInsufficientStock_ShouldReturnFalse()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 3;
        product.IsActive = true;
        var createdProduct = await _repository.CreateAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.IsAvailableAsync(createdProduct.Id, 5);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that UpdateStockAsync successfully updates product stock quantity.
    /// </summary>
    [Fact(DisplayName = "Given valid product and stock When updating stock Then should update quantity")]
    public async Task UpdateStockAsync_WithValidData_ShouldUpdateQuantity()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 10;
        product.IsActive = true;
        var createdProduct = await _repository.CreateAsync(product);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateStockAsync(createdProduct.Id, 20);
        await _context.SaveChangesAsync();

        // Assert
        var updatedProduct = await _repository.GetByIdAsync(createdProduct.Id);
        updatedProduct!.StockQuantity.Should().Be(20);
    }

    /// <summary>
    /// Tests that ReduceStockAsync successfully reduces product stock quantity.
    /// </summary>
    [Fact(DisplayName = "Given valid product and quantity When reducing stock Then should decrease quantity")]
    public async Task ReduceStockAsync_WithValidQuantity_ShouldDecreaseStock()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 15;
        product.IsActive = true;
        var createdProduct = await _repository.CreateAsync(product);
        await _context.SaveChangesAsync();

        // Act
        await _repository.ReduceStockAsync(createdProduct.Id, 5);
        await _context.SaveChangesAsync();

        // Assert
        var updatedProduct = await _repository.GetByIdAsync(createdProduct.Id);
        updatedProduct!.StockQuantity.Should().Be(10);
    }

    /// <summary>
    /// Tests that SearchByNameAsync returns products matching search term.
    /// </summary>
    [Fact(DisplayName = "Given products with various names When searching by name Then should return matching products")]
    public async Task SearchByNameAsync_WithSearchTerm_ShouldReturnMatchingProducts()
    {
        // Arrange
        var product1 = ProductTestData.GenerateValidProduct();
        product1.Name = "Laptop Computer";
        var product2 = ProductTestData.GenerateValidProduct();
        product2.Name = "Desktop Computer";
        var product3 = ProductTestData.GenerateValidProduct();
        product3.Name = "Mobile Phone";

        await _repository.CreateAsync(product1);
        await _repository.CreateAsync(product2);
        await _repository.CreateAsync(product3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.SearchByNameAsync("Computer");

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(p => p.Name.Contains("Computer"));
    }

    /// <summary>
    /// Tests that GetByPriceRangeAsync returns products within specified price range.
    /// </summary>
    [Fact(DisplayName = "Given products with various prices When getting by price range Then should return products in range")]
    public async Task GetByPriceRangeAsync_WithValidRange_ShouldReturnProductsInRange()
    {
        // Arrange
        var product1 = ProductTestData.GenerateValidProduct();
        product1.Price = 10.00m;
        var product2 = ProductTestData.GenerateValidProduct();
        product2.Price = 25.50m;
        var product3 = ProductTestData.GenerateValidProduct();
        product3.Price = 35.00m;

        await _repository.CreateAsync(product1);
        await _repository.CreateAsync(product2);
        await _repository.CreateAsync(product3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetByPriceRangeAsync(20.00m, 30.00m);

        // Assert
        results.Should().HaveCount(1);
        results.First().Price.Should().Be(25.50m);
    }



    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}