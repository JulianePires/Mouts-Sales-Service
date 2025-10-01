using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Product entity class.
/// Tests cover entity creation, validation, business rules, stock management, and state management.
/// </summary>
public class ProductTests
{
    /// <summary>
    /// Tests that a valid product can be created successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When creating product Then product should be created successfully")]
    public void Given_ValidProductData_When_CreatingProduct_Then_ProductShouldBeCreatedSuccessfully()
    {
        // Arrange
        var name = "Test Product";
        var price = 29.99m;
        var description = "A test product";
        var category = "Electronics";
        var stockQuantity = 50;
        var minStockLevel = 10;
        var image = "https://example.com/image.jpg";

        // Act
        var product = Product.Create(name, price, description, category, stockQuantity, minStockLevel, image);

        // Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(name);
        product.Price.Should().Be(price);
        product.Description.Should().Be(description);
        product.Category.Should().Be(category);
        product.StockQuantity.Should().Be(stockQuantity);
        product.MinStockLevel.Should().Be(minStockLevel);
        product.Image.Should().Be(image);
        product.IsActive.Should().BeTrue();
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that product creation fails with invalid name.
    /// </summary>
    [Fact(DisplayName = "Given empty name When creating product Then should throw ArgumentException")]
    public void Given_EmptyName_When_CreatingProduct_Then_ShouldThrowArgumentException()
    {
        // Act & Assert
        var action = () => Product.Create("", 29.99m, "Description", "Category");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Product name cannot be null or empty.*");
    }

    /// <summary>
    /// Tests that product creation fails with invalid price.
    /// </summary>
    [Fact(DisplayName = "Given zero price When creating product Then should throw ArgumentException")]
    public void Given_ZeroPrice_When_CreatingProduct_Then_ShouldThrowArgumentException()
    {
        // Act & Assert
        var action = () => Product.Create("Product", 0m, "Description", "Category");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Product price must be greater than zero.*");
    }

    /// <summary>
    /// Tests that product creation fails with negative stock quantity.
    /// </summary>
    [Fact(DisplayName = "Given negative stock quantity When creating product Then should throw ArgumentException")]
    public void Given_NegativeStockQuantity_When_CreatingProduct_Then_ShouldThrowArgumentException()
    {
        // Act & Assert
        var action = () => Product.Create("Product", 29.99m, "Description", "Category", -5);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Stock quantity cannot be negative.*");
    }

    /// <summary>
    /// Tests that stock can be added successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product When adding stock Then stock should be updated")]
    public void Given_ValidProduct_When_AddingStock_Then_StockShouldBeUpdated()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var originalStock = product.StockQuantity;
        var addQuantity = 20;

        // Act
        product.AddStock(addQuantity);

        // Assert
        product.StockQuantity.Should().Be(originalStock + addQuantity);
        product.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that adding invalid stock quantity throws exception.
    /// </summary>
    [Fact(DisplayName = "Given zero quantity When adding stock Then should throw ArgumentException")]
    public void Given_ZeroQuantity_When_AddingStock_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        var action = () => product.AddStock(0);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Quantity to add must be positive.*");
    }

    /// <summary>
    /// Tests that stock can be removed successfully.
    /// </summary>
    [Fact(DisplayName = "Given product with sufficient stock When removing stock Then stock should be updated")]
    public void Given_ProductWithSufficientStock_When_RemovingStock_Then_StockShouldBeUpdated()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 50;
        var removeQuantity = 20;

        // Act
        product.RemoveStock(removeQuantity);

        // Assert
        product.StockQuantity.Should().Be(30);
        product.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that removing more stock than available throws exception.
    /// </summary>
    [Fact(DisplayName = "Given insufficient stock When removing stock Then should throw ArgumentException")]
    public void Given_InsufficientStock_When_RemovingStock_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 10;

        // Act & Assert
        var action = () => product.RemoveStock(20);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Cannot remove more stock than available.*");
    }

    /// <summary>
    /// Tests that product price can be updated successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product When updating price Then price should be updated")]
    public void Given_ValidProduct_When_UpdatingPrice_Then_PriceShouldBeUpdated()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var newPrice = 39.99m;

        // Act
        product.UpdatePrice(newPrice);

        // Assert
        product.Price.Should().Be(newPrice);
        product.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that updating price with invalid value throws exception.
    /// </summary>
    [Fact(DisplayName = "Given zero price When updating price Then should throw ArgumentException")]
    public void Given_ZeroPrice_When_UpdatingPrice_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        var action = () => product.UpdatePrice(0m);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Product price must be greater than zero.*");
    }

    /// <summary>
    /// Tests that low stock detection works correctly.
    /// </summary>
    [Fact(DisplayName = "Given product with low stock When checking stock level Then should return true")]
    public void Given_ProductWithLowStock_When_CheckingStockLevel_Then_ShouldReturnTrue()
    {
        // Arrange
        var product = ProductTestData.GenerateLowStockProduct();

        // Act
        var isLowStock = product.IsLowStock();

        // Assert
        isLowStock.Should().BeTrue();
    }

    /// <summary>
    /// Tests that out of stock detection works correctly.
    /// </summary>
    [Fact(DisplayName = "Given out of stock product When checking availability Then should return true")]
    public void Given_OutOfStockProduct_When_CheckingAvailability_Then_ShouldReturnTrue()
    {
        // Arrange
        var product = ProductTestData.GenerateOutOfStockProduct();

        // Act
        var isOutOfStock = product.IsOutOfStock();

        // Assert
        isOutOfStock.Should().BeTrue();
    }

    /// <summary>
    /// Tests that availability for sale works correctly for active products with stock.
    /// </summary>
    [Fact(DisplayName = "Given active product with stock When checking availability for sale Then should return true")]
    public void Given_ActiveProductWithStock_When_CheckingAvailabilityForSale_Then_ShouldReturnTrue()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 10;

        // Act
        var isAvailable = product.IsAvailableForSale();

        // Assert
        isAvailable.Should().BeTrue();
    }

    /// <summary>
    /// Tests that availability for sale works correctly for inactive products.
    /// </summary>
    [Fact(DisplayName = "Given inactive product When checking availability for sale Then should return false")]
    public void Given_InactiveProduct_When_CheckingAvailabilityForSale_Then_ShouldReturnFalse()
    {
        // Arrange
        var product = ProductTestData.GenerateInactiveProduct();

        // Act
        var isAvailable = product.IsAvailableForSale();

        // Assert
        isAvailable.Should().BeFalse();
    }

    /// <summary>
    /// Tests that product can be activated successfully.
    /// </summary>
    [Fact(DisplayName = "Given inactive product When activating Then product should be active")]
    public void Given_InactiveProduct_When_Activating_Then_ProductShouldBeActive()
    {
        // Arrange
        var product = ProductTestData.GenerateInactiveProduct();

        // Act
        product.Activate();

        // Assert
        product.IsActive.Should().BeTrue();
        product.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that product can be deactivated successfully.
    /// </summary>
    [Fact(DisplayName = "Given active product When deactivating Then product should be inactive")]
    public void Given_ActiveProduct_When_Deactivating_Then_ProductShouldBeInactive()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act
        product.Deactivate();

        // Assert
        product.IsActive.Should().BeFalse();
        product.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that valid product passes validation.
    /// </summary>
    [Fact(DisplayName = "Given valid product When validating Then validation should pass")]
    public void Given_ValidProduct_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act
        var result = product.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that product with invalid name fails validation.
    /// </summary>
    [Fact(DisplayName = "Given product with invalid name When validating Then validation should fail")]
    public void Given_ProductWithInvalidName_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithInvalidName();

        // Act
        var result = product.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Error == nameof(Product.Name));
    }

    /// <summary>
    /// Tests that product with invalid price fails validation.
    /// </summary>
    [Fact(DisplayName = "Given product with invalid price When validating Then validation should fail")]
    public void Given_ProductWithInvalidPrice_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var product = ProductTestData.GenerateProductWithInvalidPrice();

        // Act
        var result = product.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Error == nameof(Product.Price));
    }

    /// <summary>
    /// Tests that product interface implementation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given product When accessing via interface Then properties should be exposed correctly")]
    public void Given_Product_When_AccessingViaInterface_Then_PropertiesShouldBeExposedCorrectly()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act
        var iProduct = (IProduct)product;

        // Assert
        iProduct.Id.Should().Be(product.Id.ToString());
        iProduct.Name.Should().Be(product.Name);
        iProduct.Price.Should().Be(product.Price);
        iProduct.Description.Should().Be(product.Description);
        iProduct.Category.Should().Be(product.Category);
        iProduct.Image.Should().Be(product.Image);
        iProduct.StockQuantity.Should().Be(product.StockQuantity);
        iProduct.MinStockLevel.Should().Be(product.MinStockLevel);
        iProduct.IsActive.Should().Be(product.IsActive);
        iProduct.CreatedAt.Should().Be(product.CreatedAt);
    }
}
