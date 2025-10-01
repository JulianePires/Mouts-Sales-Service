using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the ProductValidator class.
/// Tests cover validation of all product properties including name, price,
/// category, description, stock quantities, image, and creation date requirements.
/// </summary>
public class ProductValidatorTests
{
    private readonly ProductValidator _validator;

    public ProductValidatorTests()
    {
        _validator = new ProductValidator();
    }

    /// <summary>
    /// Tests that validation passes when all product properties are valid.
    /// </summary>
    [Fact(DisplayName = "Valid product should pass all validation rules")]
    public void Given_ValidProduct_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when product name is empty.
    /// </summary>
    [Fact(DisplayName = "Empty product name should fail validation")]
    public void Given_EmptyProductName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Name = string.Empty;

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Name)
            .WithErrorMessage("Product name is required.");
    }

    /// <summary>
    /// Tests that validation fails when product name is too short.
    /// </summary>
    [Fact(DisplayName = "Product name too short should fail validation")]
    public void Given_ProductNameTooShort_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Name = "A";

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Name)
            .WithErrorMessage("Product name must be at least 2 characters long.");
    }

    /// <summary>
    /// Tests that validation fails when product name is too long.
    /// </summary>
    [Fact(DisplayName = "Product name too long should fail validation")]
    public void Given_ProductNameTooLong_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Name = new string('A', 101);

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Name)
            .WithErrorMessage("Product name cannot be longer than 100 characters.");
    }

    /// <summary>
    /// Tests that validation fails when product price is zero.
    /// </summary>
    [Fact(DisplayName = "Zero product price should fail validation")]
    public void Given_ZeroProductPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Price = 0m;

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Price)
            .WithErrorMessage("Product price must be greater than zero.");
    }

    /// <summary>
    /// Tests that validation fails when product price is negative.
    /// </summary>
    [Fact(DisplayName = "Negative product price should fail validation")]
    public void Given_NegativeProductPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Price = -10.50m;

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Price)
            .WithErrorMessage("Product price must be greater than zero.");
    }

    /// <summary>
    /// Tests that validation fails when product price exceeds maximum limit.
    /// </summary>
    [Fact(DisplayName = "Product price exceeding limit should fail validation")]
    public void Given_ProductPriceExceedingLimit_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Price = 1000001m;

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Price)
            .WithErrorMessage("Product price cannot exceed 1,000,000.");
    }

    /// <summary>
    /// Tests that validation fails when product category is empty.
    /// </summary>
    [Fact(DisplayName = "Empty product category should fail validation")]
    public void Given_EmptyProductCategory_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Category = string.Empty;

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Category)
            .WithErrorMessage("Product category is required.");
    }

    /// <summary>
    /// Tests that validation fails when product category is too long.
    /// </summary>
    [Fact(DisplayName = "Product category too long should fail validation")]
    public void Given_ProductCategoryTooLong_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Category = new string('A', 51);

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Category)
            .WithErrorMessage("Product category cannot be longer than 50 characters.");
    }

    /// <summary>
    /// Tests that validation fails when description is too long.
    /// </summary>
    [Fact(DisplayName = "Product description too long should fail validation")]
    public void Given_ProductDescriptionTooLong_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Description = new string('A', 501);

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Description)
            .WithErrorMessage("Product description cannot be longer than 500 characters.");
    }

    /// <summary>
    /// Tests that validation passes when description is empty (optional field).
    /// </summary>
    [Fact(DisplayName = "Empty description should pass validation")]
    public void Given_EmptyDescription_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Description = string.Empty;

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.Description);
    }

    /// <summary>
    /// Tests that validation fails when stock quantity is negative.
    /// </summary>
    [Fact(DisplayName = "Negative stock quantity should fail validation")]
    public void Given_NegativeStockQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = -5;

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.StockQuantity)
            .WithErrorMessage("Stock quantity cannot be negative.");
    }

    /// <summary>
    /// Tests that validation fails when minimum stock level is negative.
    /// </summary>
    [Fact(DisplayName = "Negative minimum stock level should fail validation")]
    public void Given_NegativeMinStockLevel_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.MinStockLevel = -3;

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.MinStockLevel)
            .WithErrorMessage("Minimum stock level cannot be negative.");
    }

    /// <summary>
    /// Tests that validation fails when image URL is too long.
    /// </summary>
    [Fact(DisplayName = "Image URL too long should fail validation")]
    public void Given_ImageUrlTooLong_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.Image = new string('A', 301);

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Image)
            .WithErrorMessage("Product image URL cannot be longer than 300 characters.");
    }

    /// <summary>
    /// Tests that validation fails when creation date is in the future.
    /// </summary>
    [Fact(DisplayName = "Future creation date should fail validation")]
    public void Given_FutureCreationDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        product.CreatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.CreatedAt)
            .WithErrorMessage("Product creation date cannot be in the future.");
    }
}
