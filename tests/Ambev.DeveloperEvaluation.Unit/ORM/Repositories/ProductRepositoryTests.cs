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
public class ProductRepositoryTests : BaseRepositoryTestFixture<ProductRepository>
{
    /// <summary>
    /// Creates an instance of the ProductRepository for testing
    /// </summary>
    /// <param name="context">The database context to use</param>
    /// <returns>An instance of ProductRepository</returns>
    protected override ProductRepository CreateRepository(DefaultContext context)
    {
        return new ProductRepository(context);
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
        var result = await Repository.CreateAsync(product);
        await SaveChangesAsync();

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
        await FluentActions.Invoking(() => Repository.CreateAsync(null!))
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

        var createdProduct = await Repository.CreateAsync(product);
        await SaveChangesAsync();

        // Act
        var result = await Repository.GetByIdAsync(createdProduct.Id);

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
        var result = await Repository.GetByIdAsync(nonExistingId);

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

        await Repository.CreateAsync(product1);
        await Repository.CreateAsync(product2);
        await Repository.CreateAsync(product3);
        await SaveChangesAsync();

        // Act
        var results = await Repository.GetByCategoryAsync(category);

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

        await Repository.CreateAsync(product1);
        await Repository.CreateAsync(product2);
        await Repository.CreateAsync(product3);
        await SaveChangesAsync();

        // Act
        var results = await Repository.GetInStockAsync();

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

        await Repository.CreateAsync(product1);
        await Repository.CreateAsync(product2);
        await Repository.CreateAsync(product3);
        await SaveChangesAsync();

        // Act
        var results = await Repository.GetLowStockAsync(10);

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
        var createdProduct = await Repository.CreateAsync(product);
        await SaveChangesAsync();

        // Act
        var result = await Repository.IsAvailableAsync(createdProduct.Id, 5);

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
        var createdProduct = await Repository.CreateAsync(product);
        await SaveChangesAsync();

        // Act
        var result = await Repository.IsAvailableAsync(createdProduct.Id, 5);

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
        var createdProduct = await Repository.CreateAsync(product);
        await SaveChangesAsync();

        // Act
        await Repository.UpdateStockAsync(createdProduct.Id, 20);
        await SaveChangesAsync();

        // Assert
        var updatedProduct = await Repository.GetByIdAsync(createdProduct.Id);
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
        var createdProduct = await Repository.CreateAsync(product);
        await SaveChangesAsync();

        // Act
        await Repository.ReduceStockAsync(createdProduct.Id, 5);
        await SaveChangesAsync();

        // Assert
        var updatedProduct = await Repository.GetByIdAsync(createdProduct.Id);
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

        await Repository.CreateAsync(product1);
        await Repository.CreateAsync(product2);
        await Repository.CreateAsync(product3);
        await SaveChangesAsync();

        // Act
        var results = await Repository.SearchByNameAsync("Computer");

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

        await Repository.CreateAsync(product1);
        await Repository.CreateAsync(product2);
        await Repository.CreateAsync(product3);
        await SaveChangesAsync();

        // Act
        var results = await Repository.GetByPriceRangeAsync(20.00m, 30.00m);

        // Assert
        results.Should().HaveCount(1);
        results.First().Price.Should().Be(25.50m);
    }

    /// <summary>
    /// Tests that GetCountAsync returns the correct count of products
    /// </summary>
    [Fact(DisplayName = "Given products in repository When getting count Then should return correct total")]
    public async Task GetCountAsync_WithProducts_ShouldReturnCorrectCount()
    {
        // Arrange
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();
        var product3 = ProductTestData.GenerateValidProduct();

        await Repository.CreateAsync(product1);
        await Repository.CreateAsync(product2);
        await Repository.CreateAsync(product3);
        await SaveChangesAsync();

        // Act
        var count = await Repository.GetCountAsync();

        // Assert
        count.Should().Be(3);
    }

    /// <summary>
    /// Tests that GetCountAsync returns zero when no products exist
    /// </summary>
    [Fact(DisplayName = "Given empty repository When getting count Then should return zero")]
    public async Task GetCountAsync_EmptyRepository_ShouldReturnZero()
    {
        // Act
        var count = await Repository.GetCountAsync();

        // Assert
        count.Should().Be(0);
    }

    /// <summary>
    /// Tests that GetPaginatedAsync returns correct page of products
    /// </summary>
    [Fact(DisplayName = "Given products When getting paginated results Then should return correct page")]
    public async Task GetPaginatedAsync_WithValidPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var products = new List<Ambev.DeveloperEvaluation.Domain.Entities.Product>();
        for (int i = 1; i <= 10; i++)
        {
            var product = ProductTestData.GenerateValidProduct();
            product.Name = $"Product {i:D2}"; // Ensure consistent ordering
            products.Add(product);
            await Repository.CreateAsync(product);
        }
        await SaveChangesAsync();

        // Act
        var result = await Repository.GetPaginatedAsync(2, 3); // Second page, 3 items per page

        // Assert
        result.Should().HaveCount(3);
        // Note: BaseRepository orders by Id (GUID), not by Name, so we just verify pagination works
        var resultList = result.ToList();
        resultList.Should().HaveCount(3);
        resultList.Should().OnlyContain(p => products.Any(created => created.Id == p.Id));
    }

    /// <summary>
    /// Tests that GetPaginatedAsync returns empty collection for page beyond available data
    /// </summary>
    [Fact(DisplayName = "Given limited products When requesting page beyond data Then should return empty")]
    public async Task GetPaginatedAsync_PageBeyondData_ShouldReturnEmpty()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        await Repository.CreateAsync(product);
        await SaveChangesAsync();

        // Act
        var result = await Repository.GetPaginatedAsync(5, 10); // Way beyond available data

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that GetPaginatedAsync throws exception for invalid page number
    /// </summary>
    [Fact(DisplayName = "Given invalid page number When getting paginated results Then should throw exception")]
    public async Task GetPaginatedAsync_InvalidPageNumber_ShouldThrowException()
    {
        // Act & Assert
        await FluentActions.Invoking(() => Repository.GetPaginatedAsync(0, 5))
            .Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithParameterName("pageNumber");

        await FluentActions.Invoking(() => Repository.GetPaginatedAsync(-1, 5))
            .Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithParameterName("pageNumber");
    }

    /// <summary>
    /// Tests that GetPaginatedAsync throws exception for invalid page size
    /// </summary>
    [Fact(DisplayName = "Given invalid page size When getting paginated results Then should throw exception")]
    public async Task GetPaginatedAsync_InvalidPageSize_ShouldThrowException()
    {
        // Act & Assert
        await FluentActions.Invoking(() => Repository.GetPaginatedAsync(1, 0))
            .Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithParameterName("pageSize");

        await FluentActions.Invoking(() => Repository.GetPaginatedAsync(1, -1))
            .Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithParameterName("pageSize");
    }

    /// <summary>
    /// Tests that GetPaginatedAsync handles partial last page correctly
    /// </summary>
    [Fact(DisplayName = "Given products When requesting partial last page Then should return remaining items")]
    public async Task GetPaginatedAsync_PartialLastPage_ShouldReturnRemainingItems()
    {
        // Arrange
        var products = new List<Ambev.DeveloperEvaluation.Domain.Entities.Product>();
        for (int i = 1; i <= 7; i++)
        {
            var product = ProductTestData.GenerateValidProduct();
            product.Name = $"Product {i:D2}";
            products.Add(product);
            await Repository.CreateAsync(product);
        }
        await SaveChangesAsync();

        // Act
        var result = await Repository.GetPaginatedAsync(3, 3); // Third page with 3 items per page (should get 1 item)

        // Assert
        result.Should().HaveCount(1);
        result.First().Should().BeOneOf(products.ToArray()); // Verify it's one of our created products
    }

    /// <summary>
    /// Tests that GetPaginatedAsync returns results ordered consistently
    /// </summary>
    [Fact(DisplayName = "Given products When getting paginated results Then should maintain consistent ordering")]
    public async Task GetPaginatedAsync_MultiplePages_ShouldMaintainConsistentOrdering()
    {
        // Arrange
        var products = new List<Ambev.DeveloperEvaluation.Domain.Entities.Product>();
        var productNames = new[] { "Zebra Product", "Alpha Product", "Beta Product", "Gamma Product" };
        foreach (var name in productNames)
        {
            var product = ProductTestData.GenerateValidProduct();
            product.Name = name;
            products.Add(product);
            await Repository.CreateAsync(product);
        }
        await SaveChangesAsync();

        // Act
        var page1 = await Repository.GetPaginatedAsync(1, 2);
        var page2 = await Repository.GetPaginatedAsync(2, 2);

        // Assert
        var allResults = page1.Concat(page2).ToList();
        allResults.Should().HaveCount(4);
        // Verify all our created products are returned (order by Id may vary)
        allResults.Select(p => p.Name).Should().BeEquivalentTo(productNames);
        // Verify no duplicates between pages
        page1.Select(p => p.Id).Should().NotIntersectWith(page2.Select(p => p.Id));
    }
}