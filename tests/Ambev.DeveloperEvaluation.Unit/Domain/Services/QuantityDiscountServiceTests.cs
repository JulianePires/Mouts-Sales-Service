using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Services;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services;

/// <summary>
/// Contains unit tests for the QuantityDiscountService class.
/// Tests cover discount calculations, validation rules, and edge cases.
/// </summary>
public class QuantityDiscountServiceTests
{
    private readonly IDiscountService _discountService;

    public QuantityDiscountServiceTests()
    {
        _discountService = new QuantityDiscountService();
    }

    [Theory(DisplayName = "Given various quantities When calculating discount Then should return correct values")]
    [InlineData(1, 100, 0, 0, "1 item - sem desconto")]
    [InlineData(3, 100, 0, 0, "3 itens - sem desconto")]
    [InlineData(4, 100, 10, 40, "4 itens - 10% desconto")]
    [InlineData(9, 100, 10, 90, "9 itens - 10% desconto")]
    [InlineData(10, 100, 20, 200, "10 itens - 20% desconto")]
    [InlineData(15, 100, 20, 300, "15 itens - 20% desconto")]
    [InlineData(20, 100, 20, 400, "20 itens - 20% desconto")]
    public void CalculateDiscount_VariousQuantities_ShouldReturnCorrectValues(
        int quantity,
        decimal unitPrice,
        decimal expectedPercentage,
        decimal expectedAmount,
        string description)
    {
        // Act
        var discount = _discountService.CalculateDiscount(quantity, unitPrice);

        // Assert
        discount.Percentage.Should().Be(expectedPercentage, description);
        discount.Amount.Should().Be(expectedAmount, description);
    }

    [Fact(DisplayName = "Given quantity above 20 When calculating discount Then should throw exception")]
    public void CalculateDiscount_QuantityAbove20_ShouldThrowException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _discountService.CalculateDiscount(21, 100));

        exception.Message.Should().Contain("Não é possível vender mais de 20 itens idênticos");
    }

    [Theory(DisplayName = "Given invalid quantity When calculating discount Then should throw exception")]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateDiscount_InvalidQuantity_ShouldThrowException(int invalidQuantity)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _discountService.CalculateDiscount(invalidQuantity, 100));

        exception.Message.Should().Contain("Quantidade deve ser maior que zero");
    }

    [Theory(DisplayName = "Given invalid price When calculating discount Then should throw exception")]
    [InlineData(0)]
    [InlineData(-10.50)]
    public void CalculateDiscount_InvalidPrice_ShouldThrowException(decimal invalidPrice)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _discountService.CalculateDiscount(5, invalidPrice));

        exception.Message.Should().Contain("Preço unitário deve ser maior que zero");
    }

    [Fact(DisplayName = "Given price just below maximum When calculating discount Then should NOT throw exception")]
    public void CalculateDiscount_PriceJustBelowMaximum_ShouldNotThrowException()
    {
        // Arrange
        decimal maxPrice = BusinessConstants.MaximumUnitPrice;
        decimal priceJustBelowMax = maxPrice - 0.01m;

        // Act & Assert
        var exception = Record.Exception(() => _discountService.CalculateDiscount(5, priceJustBelowMax));

        exception.Should().BeNull("because price just below the maximum should be valid");
    }

    [Fact(DisplayName = "Given price above limit When calculating discount Then should throw exception")]
    public void CalculateDiscount_PriceAboveLimit_ShouldThrowException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _discountService.CalculateDiscount(5, 1_000_001m));

        exception.Message.Should().Contain("excede o limite máximo permitido");
    }

    [Fact(DisplayName = "Given maximum allowed price When calculating discount Then should succeed")]
    public void CalculateDiscount_MaximumAllowedPrice_ShouldSucceed()
    {
        // Arrange
        var quantity = 10;
        var maxPrice = 1_000_000m;
        var expectedAmount = 2_000_000m; // 20% of 10 * 1,000,000

        // Act
        var discount = _discountService.CalculateDiscount(quantity, maxPrice);

        // Assert
        discount.Percentage.Should().Be(20m);
        discount.Amount.Should().Be(expectedAmount);
    }

    [Theory(DisplayName = "Given various quantities When checking eligibility Then should return correct value")]
    [InlineData(1, false)]
    [InlineData(3, false)]
    [InlineData(4, true)]
    [InlineData(10, true)]
    [InlineData(20, true)]
    public void IsEligibleForDiscount_ShouldReturnCorrectValue(int quantity, bool expected)
    {
        // Act
        var result = _discountService.IsEligibleForDiscount(quantity);

        // Assert
        result.Should().Be(expected);
    }

    [Fact(DisplayName = "Given quantity above limit When checking eligibility Then should return false")]
    public void IsEligibleForDiscount_QuantityAboveLimit_ShouldReturnFalse()
    {
        // Act
        var result = _discountService.IsEligibleForDiscount(21);

        // Assert
        result.Should().BeFalse();
    }

    [Theory(DisplayName = "Given valid quantity When validating limits Then should not throw")]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    public void ValidateQuantityLimits_ValidQuantity_ShouldNotThrow(int validQuantity)
    {
        // Act & Assert
        var action = () => _discountService.ValidateQuantityLimits(validQuantity);
        action.Should().NotThrow();
    }

    [Theory(DisplayName = "Given invalid quantity When validating limits Then should throw exception")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(21)]
    public void ValidateQuantityLimits_InvalidQuantity_ShouldThrowException(int invalidQuantity)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _discountService.ValidateQuantityLimits(invalidQuantity));

        if (invalidQuantity <= 0)
            exception.Message.Should().Contain("Quantidade deve ser maior que zero");
        else
            exception.Message.Should().Contain("Não é possível vender mais de 20 itens idênticos");
    }

    [Fact(DisplayName = "Given decimal prices When calculating discount Then should handle precision correctly")]
    public void CalculateDiscount_DecimalPrices_ShouldHandlePrecisionCorrectly()
    {
        // Arrange
        var quantity = 10;
        var unitPrice = 33.33m;
        var expectedAmount = 66.66m; // 20% of 333.30

        // Act
        var discount = _discountService.CalculateDiscount(quantity, unitPrice);

        // Assert
        discount.Percentage.Should().Be(20m);
        discount.Amount.Should().Be(expectedAmount);
    }
}