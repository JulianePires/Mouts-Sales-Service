using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleItemValidator class.
/// Tests cover validation of all sale item properties including sale ID, product,
/// quantity, unit price, discount percent, and business rule validations for discount calculations.
/// </summary>
public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator;

    public SaleItemValidatorTests()
    {
        _validator = new SaleItemValidator();
    }

    /// <summary>
    /// Tests that validation passes when all sale item properties are valid.
    /// </summary>
    [Fact(DisplayName = "Valid sale item should pass all validation rules")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when sale ID is empty.
    /// </summary>
    [Fact(DisplayName = "Empty sale ID should fail validation")]
    public void Given_EmptySaleId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.SaleId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.SaleId)
            .WithErrorMessage("Sale ID is required.");
    }

    /// <summary>
    /// Tests that validation fails when product is null.
    /// </summary>
    [Fact(DisplayName = "Null product should fail validation")]
    public void Given_NullProduct_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Product = null!;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.Product)
            .WithErrorMessage("Product is required.");
    }

    /// <summary>
    /// Tests that validation fails when quantity is zero.
    /// </summary>
    [Fact(DisplayName = "Zero quantity should fail validation")]
    public void Given_ZeroQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Quantity = 0;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.Quantity)
            .WithErrorMessage("Quantity must be greater than zero.");
    }

    /// <summary>
    /// Tests that validation fails when quantity is negative.
    /// </summary>
    [Fact(DisplayName = "Negative quantity should fail validation")]
    public void Given_NegativeQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Quantity = -5;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.Quantity)
            .WithErrorMessage("Quantity must be greater than zero.");
    }

    /// <summary>
    /// Tests that validation fails when quantity exceeds maximum limit (20).
    /// </summary>
    [Fact(DisplayName = "Quantity over 20 should fail validation")]
    public void Given_QuantityOver20_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Quantity = 21;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.Quantity)
            .WithErrorMessage("Cannot sell more than 20 units of the same product in a single sale.");
    }

    /// <summary>
    /// Tests that validation fails when unit price is zero.
    /// </summary>
    [Fact(DisplayName = "Zero unit price should fail validation")]
    public void Given_ZeroUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.UnitPrice = 0m;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.UnitPrice)
            .WithErrorMessage("Unit price must be greater than zero.");
    }

    /// <summary>
    /// Tests that validation fails when unit price is negative.
    /// </summary>
    [Fact(DisplayName = "Negative unit price should fail validation")]
    public void Given_NegativeUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.UnitPrice = -10.50m;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.UnitPrice)
            .WithErrorMessage("Unit price must be greater than zero.");
    }

    /// <summary>
    /// Tests that validation fails when unit price exceeds maximum limit.
    /// </summary>
    [Fact(DisplayName = "Unit price exceeding limit should fail validation")]
    public void Given_UnitPriceExceedingLimit_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.UnitPrice = 1000001m;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.UnitPrice)
            .WithErrorMessage("Unit price cannot exceed 1,000,000.");
    }

    /// <summary>
    /// Tests that validation fails when discount percent is negative.
    /// </summary>
    [Fact(DisplayName = "Negative discount percent should fail validation")]
    public void Given_NegativeDiscountPercent_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.DiscountPercent = -5m;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.DiscountPercent)
            .WithErrorMessage("Discount percentage cannot be negative.");
    }

    /// <summary>
    /// Tests that validation fails when discount percent exceeds 100.
    /// </summary>
    [Fact(DisplayName = "Discount percent over 100 should fail validation")]
    public void Given_DiscountPercentOver100_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.DiscountPercent = 105m;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.DiscountPercent)
            .WithErrorMessage("Discount percentage cannot exceed 100%.");
    }

    /// <summary>
    /// Tests that validation passes when total price is zero for cancelled items.
    /// </summary>
    [Fact(DisplayName = "Zero total price for cancelled item should pass validation")]
    public void Given_CancelledItemWithZeroTotalPrice_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateCancelledSaleItem();

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldNotHaveValidationErrorFor(si => si.TotalPrice);
    }

    /// <summary>
    /// Tests that validation fails when creation date is in the future.
    /// </summary>
    [Fact(DisplayName = "Future creation date should fail validation")]
    public void Given_FutureCreationDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.CreatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.CreatedAt)
            .WithErrorMessage("Sale item creation date cannot be in the future.");
    }

    /// <summary>
    /// Tests business rule: discount should match quantity for 4-9 items (10% discount).
    /// </summary>
    [Theory(DisplayName = "Discount should match quantity for 4-9 items")]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(9)]
    public void Given_Quantity4To9WithWrongDiscount_When_Validated_Then_ShouldHaveError(int quantity)
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Quantity = quantity;
        saleItem.DiscountPercent = 20m; // Wrong discount (should be 10%)

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si)
            .WithErrorMessage("Discount percentage does not match the quantity rules.");
    }

    /// <summary>
    /// Tests business rule: discount should match quantity for 10-20 items (20% discount).
    /// </summary>
    [Theory(DisplayName = "Discount should match quantity for 10-20 items")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void Given_Quantity10To20WithWrongDiscount_When_Validated_Then_ShouldHaveError(int quantity)
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Quantity = quantity;
        saleItem.DiscountPercent = 10m; // Wrong discount (should be 20%)

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si)
            .WithErrorMessage("Discount percentage does not match the quantity rules.");
    }

    /// <summary>
    /// Tests business rule: discount should match quantity for 1-3 items (0% discount).
    /// </summary>
    [Theory(DisplayName = "Discount should match quantity for 1-3 items")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Given_Quantity1To3WithWrongDiscount_When_Validated_Then_ShouldHaveError(int quantity)
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Quantity = quantity;
        saleItem.DiscountPercent = 10m; // Wrong discount (should be 0%)

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si)
            .WithErrorMessage("Discount percentage does not match the quantity rules.");
    }

    /// <summary>
    /// Tests business rule: correct discount validation passes for different quantity ranges.
    /// </summary>
    [Theory(DisplayName = "Correct discount for quantity should pass validation")]
    [InlineData(2, 0)] // 1-3 items = 0%
    [InlineData(5, 10)] // 4-9 items = 10%
    [InlineData(15, 20)] // 10-20 items = 20%
    public void Given_CorrectDiscountForQuantity_When_Validated_Then_ShouldNotHaveError(int quantity, decimal expectedDiscount)
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Quantity = quantity;
        saleItem.DiscountPercent = expectedDiscount;

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldNotHaveValidationErrorFor(si => si);
    }
}
