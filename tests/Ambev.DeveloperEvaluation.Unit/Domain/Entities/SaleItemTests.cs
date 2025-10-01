using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover entity creation, discount calculations, business rules validation, and state management.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that a valid sale item can be created successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale item data When creating sale item Then sale item should be created successfully")]
    public void Given_ValidSaleItemData_When_CreatingSaleItem_Then_SaleItemShouldBeCreatedSuccessfully()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        var quantity = 5;
        var unitPrice = 10.00m;

        // Act
        var saleItem = SaleItem.Create(saleId, product, quantity, unitPrice);

        // Assert
        saleItem.Should().NotBeNull();
        saleItem.SaleId.Should().Be(saleId);
        saleItem.Product.Should().Be(product);
        saleItem.Quantity.Should().Be(quantity);
        saleItem.UnitPrice.Should().Be(unitPrice);
        saleItem.IsCancelled.Should().BeFalse();
        saleItem.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that sale item creation fails with empty sale ID.
    /// </summary>
    [Fact(DisplayName = "Given empty sale ID When creating sale item Then should throw ArgumentException")]
    public void Given_EmptySaleId_When_CreatingSaleItem_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        var action = () => SaleItem.Create(Guid.Empty, product, 5);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Sale ID cannot be empty.*");
    }

    /// <summary>
    /// Tests that sale item creation fails with null product.
    /// </summary>
    [Fact(DisplayName = "Given null product When creating sale item Then should throw ArgumentException")]
    public void Given_NullProduct_When_CreatingSaleItem_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var saleId = Guid.NewGuid();

        // Act & Assert
        var action = () => SaleItem.Create(saleId, null!, 5);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Product cannot be null.*");
    }

    /// <summary>
    /// Tests that sale item creation fails with quantity exceeding maximum limit.
    /// </summary>
    [Fact(DisplayName = "Given quantity over 20 When creating sale item Then should throw InvalidOperationException")]
    public void Given_QuantityOver20_When_CreatingSaleItem_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        var action = () => SaleItem.Create(saleId, product, 21);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 units of the same product in a single sale.*");
    }

    /// <summary>
    /// Tests that no discount is applied for quantities 1-3.
    /// </summary>
    [Theory(DisplayName = "Given quantity 1-3 When creating sale item Then should have no discount")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Given_Quantity1To3_When_CreatingSaleItem_Then_ShouldHaveNoDiscount(int quantity)
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateSaleItemWithNoDiscount();
        saleItem.Quantity = quantity;
        // Force recalculation
        saleItem.UpdateQuantity(quantity);

        // Act & Assert
        saleItem.DiscountPercent.Should().Be(0m);
        var expectedTotal = Math.Round(quantity * saleItem.UnitPrice, 2, MidpointRounding.AwayFromZero);
        saleItem.TotalPrice.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests that 10% discount is applied for quantities 4-9.
    /// </summary>
    [Theory(DisplayName = "Given quantity 4-9 When creating sale item Then should have 10% discount")]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(9)]
    public void Given_Quantity4To9_When_CreatingSaleItem_Then_ShouldHave10PercentDiscount(int quantity)
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        product.Price = 10.00m;

        // Act
        var saleItem = SaleItem.Create(saleId, product, quantity);

        // Assert
        saleItem.DiscountPercent.Should().Be(10m);
        var expectedTotal = quantity * product.Price * 0.9m;
        saleItem.TotalPrice.Should().Be(Math.Round(expectedTotal, 2, MidpointRounding.AwayFromZero));
    }

    /// <summary>
    /// Tests that 20% discount is applied for quantities 10-20.
    /// </summary>
    [Theory(DisplayName = "Given quantity 10-20 When creating sale item Then should have 20% discount")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void Given_Quantity10To20_When_CreatingSaleItem_Then_ShouldHave20PercentDiscount(int quantity)
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        product.Price = 10.00m;

        // Act
        var saleItem = SaleItem.Create(saleId, product, quantity);

        // Assert
        saleItem.DiscountPercent.Should().Be(20m);
        var expectedTotal = quantity * product.Price * 0.8m;
        saleItem.TotalPrice.Should().Be(Math.Round(expectedTotal, 2, MidpointRounding.AwayFromZero));
    }

    /// <summary>
    /// Tests that sale item can be cancelled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale item When cancelling Then sale item should be cancelled")]
    public void Given_ValidSaleItem_When_Cancelling_Then_SaleItemShouldBeCancelled()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        saleItem.Cancel();

        // Assert
        saleItem.IsCancelled.Should().BeTrue();
        saleItem.TotalPrice.Should().Be(0);
        saleItem.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that cancelled sale item can be reactivated.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale item When reactivating Then sale item should be active")]
    public void Given_CancelledSaleItem_When_Reactivating_Then_SaleItemShouldBeActive()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateCancelledSaleItem();
        var originalTotalPrice = saleItem.Quantity * saleItem.UnitPrice;

        // Act
        saleItem.Reactivate();

        // Assert
        saleItem.IsCancelled.Should().BeFalse();
        saleItem.TotalPrice.Should().BeGreaterThan(0);
        saleItem.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that quantity can be updated successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale item When updating quantity Then quantity should be updated")]
    public void Given_ValidSaleItem_When_UpdatingQuantity_Then_QuantityShouldBeUpdated()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        var newQuantity = 8;

        // Act
        saleItem.UpdateQuantity(newQuantity);

        // Assert
        saleItem.Quantity.Should().Be(newQuantity);
        saleItem.DiscountPercent.Should().Be(10m); // 4-9 range
        saleItem.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that updating quantity on cancelled item throws exception.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale item When updating quantity Then should throw InvalidOperationException")]
    public void Given_CancelledSaleItem_When_UpdatingQuantity_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateCancelledSaleItem();

        // Act & Assert
        var action = () => saleItem.UpdateQuantity(5);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot update quantity of a cancelled item.*");
    }

    /// <summary>
    /// Tests that discount amount calculation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given sale item with discount When calculating discount amount Then should return correct amount")]
    public void Given_SaleItemWithDiscount_When_CalculatingDiscountAmount_Then_ShouldReturnCorrectAmount()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateSaleItemWith10PercentDiscount();
        var expectedDiscount = saleItem.Quantity * saleItem.UnitPrice * 0.1m;

        // Act
        var discountAmount = saleItem.GetDiscountAmount();

        // Assert
        discountAmount.Should().Be(Math.Round(expectedDiscount, 2, MidpointRounding.AwayFromZero));
    }

    /// <summary>
    /// Tests that subtotal calculation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given sale item When calculating subtotal Then should return correct subtotal")]
    public void Given_SaleItem_When_CalculatingSubtotal_Then_ShouldReturnCorrectSubtotal()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        var expectedSubtotal = saleItem.Quantity * saleItem.UnitPrice;

        // Act
        var subtotal = saleItem.GetSubtotal();

        // Assert
        subtotal.Should().Be(expectedSubtotal);
    }

    /// <summary>
    /// Tests that cancelled sale item returns zero for calculations.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale item When calculating amounts Then should return zero")]
    public void Given_CancelledSaleItem_When_CalculatingAmounts_Then_ShouldReturnZero()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateCancelledSaleItem();

        // Act & Assert
        saleItem.GetDiscountAmount().Should().Be(0);
        saleItem.GetSubtotal().Should().Be(0);
        saleItem.TotalPrice.Should().Be(0);
    }

    /// <summary>
    /// Tests that valid sale item passes validation.
    /// </summary>
    [Fact(DisplayName = "Given valid sale item When validating Then validation should pass")]
    public void Given_ValidSaleItem_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var result = saleItem.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that sale item with invalid quantity fails validation.
    /// </summary>
    [Fact(DisplayName = "Given sale item with zero quantity When validating Then validation should fail")]
    public void Given_SaleItemWithZeroQuantity_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();
        saleItem.Quantity = 0;

        // Act
        var result = saleItem.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Quantity"));
    }

    /// <summary>
    /// Tests that financial rounding works correctly for values that round both up and down.
    /// </summary>
    [Theory(DisplayName = "Given various decimal values When calculating totals Then should apply correct rounding")]
    [InlineData(3, 10.334, 31.00, "Rounding down: 3 * 10.334 = 31.002 -> 31.00 (no discount)")]
    [InlineData(3, 10.335, 31.01, "Rounding up: 3 * 10.335 = 31.005 -> 31.01 (no discount)")]
    [InlineData(2, 33.334, 66.67, "Rounding down: 2 * 33.334 = 66.668 -> 66.67 (no discount)")]
    [InlineData(2, 33.335, 66.67, "Rounding up: 2 * 33.335 = 66.670 -> 66.67 (no discount)")]
    [InlineData(3, 33.50, 100.50, "Midpoint rounding: 3 * 33.50 = 100.500 -> 100.50 (no discount)")]
    [InlineData(4, 25.125, 90.45, "With discount: 4 * 25.125 = 100.500, 10% discount = 90.45")]
    [InlineData(10, 12.50, 100.00, "With discount: 10 * 12.50 = 125.00, 20% discount = 100.00")]
    public void Given_VariousDecimalValues_When_CalculatingTotals_Then_ShouldApplyCorrectRounding(
        int quantity,
        decimal unitPrice,
        decimal expectedTotal,
        string description)
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var product = ProductTestData.GenerateValidProduct();
        product.Price = unitPrice;

        // Act
        var saleItem = SaleItem.Create(saleId, product, quantity, unitPrice);

        // Assert
        saleItem.TotalPrice.Should().Be(expectedTotal, description);
        saleItem.UnitPrice.Should().Be(unitPrice);
        saleItem.Quantity.Should().Be(quantity);
    }

    /// <summary>
    /// Tests that sale item interface implementation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given sale item When accessing via interface Then properties should be exposed correctly")]
    public void Given_SaleItem_When_AccessingViaInterface_Then_PropertiesShouldBeExposedCorrectly()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var iSaleItem = (ISaleItem)saleItem;

        // Assert
        iSaleItem.Id.Should().Be(saleItem.Id);
        iSaleItem.SaleId.Should().Be(saleItem.SaleId);
        iSaleItem.Product.Should().Be(saleItem.Product);
        iSaleItem.Quantity.Should().Be(saleItem.Quantity);
        iSaleItem.DiscountPercent.Should().Be(saleItem.DiscountPercent);
        iSaleItem.UnitPrice.Should().Be(saleItem.UnitPrice);
        iSaleItem.TotalPrice.Should().Be(saleItem.TotalPrice);
        iSaleItem.IsCancelled.Should().Be(saleItem.IsCancelled);
    }
}
