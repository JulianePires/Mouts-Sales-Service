using FluentValidation.TestHelper;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.AddSaleItem;

/// <summary>
/// Contains unit tests for the <see cref="AddSaleItemCommandValidator"/> class.
/// Tests validation rules for AddSaleItem commands.
/// </summary>
public class AddSaleItemCommandValidatorTests
{
    private readonly AddSaleItemCommandValidator _validator;

    /// <summary>
    /// Initializes a new instance of AddSaleItemCommandValidatorTests.
    /// </summary>
    public AddSaleItemCommandValidatorTests()
    {
        _validator = new AddSaleItemCommandValidator();
    }

    /// <summary>
    /// Tests that validation succeeds for a valid command.
    /// </summary>
    [Fact(DisplayName = "Should not have error when command is valid")]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5,
            UnitPrice = 10.00m
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when SaleId is empty.
    /// </summary>
    [Fact(DisplayName = "Should have error when SaleId is empty")]
    public void Should_Have_Error_When_SaleId_Is_Empty()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.Empty,
            ProductId = Guid.NewGuid(),
            Quantity = 5
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SaleId)
              .WithErrorMessage("Sale ID is required.");
    }

    /// <summary>
    /// Tests that validation fails when ProductId is empty.
    /// </summary>
    [Fact(DisplayName = "Should have error when ProductId is empty")]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.Empty,
            Quantity = 5
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId)
              .WithErrorMessage("Product ID is required.");
    }

    /// <summary>
    /// Tests that validation fails when Quantity is zero.
    /// </summary>
    [Fact(DisplayName = "Should have error when Quantity is zero")]
    public void Should_Have_Error_When_Quantity_Is_Zero()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 0
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
              .WithErrorMessage("Quantity must be greater than zero.");
    }

    /// <summary>
    /// Tests that validation fails when Quantity exceeds maximum allowed.
    /// </summary>
    [Fact(DisplayName = "Should have error when Quantity exceeds 20")]
    public void Should_Have_Error_When_Quantity_Exceeds_Twenty()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 25 // Exceeds maximum of 20
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
              .WithErrorMessage("Quantity cannot exceed 20 units per product.");
    }

    /// <summary>
    /// Tests that validation fails when UnitPrice is negative.
    /// </summary>
    [Fact(DisplayName = "Should have error when UnitPrice is negative")]
    public void Should_Have_Error_When_UnitPrice_Is_Negative()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5,
            UnitPrice = -10.00m
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UnitPrice)
              .WithErrorMessage("Unit price must be greater than zero when provided.");
    }

    /// <summary>
    /// Tests that validation succeeds when UnitPrice is null.
    /// </summary>
    [Fact(DisplayName = "Should not have error when UnitPrice is null")]
    public void Should_Not_Have_Error_When_UnitPrice_Is_Null()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5,
            UnitPrice = null
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UnitPrice);
    }
}