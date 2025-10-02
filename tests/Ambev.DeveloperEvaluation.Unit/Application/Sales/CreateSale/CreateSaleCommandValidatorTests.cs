using Xunit;
using FluentValidation.TestHelper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using SaleCommandTestData = Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData.SaleTestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleCommandValidator"/> class.
/// </summary>
public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleCommandValidatorTests"/> class.
    /// </summary>
    public CreateSaleCommandValidatorTests()
    {
        _validator = new CreateSaleCommandValidator();
    }

    /// <summary>
    /// Tests that validation succeeds when CreateSaleCommand is valid.
    /// </summary>
    [Fact(DisplayName = "Should not have error when CreateSaleCommand is valid")]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when CustomerId is empty.
    /// </summary>
    [Fact(DisplayName = "Should have error when CustomerId is empty")]
    public void Should_Have_Error_When_CustomerId_Is_Empty()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.CustomerId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    /// <summary>
    /// Tests that validation fails when BranchId is empty.
    /// </summary>
    [Fact(DisplayName = "Should have error when BranchId is empty")]
    public void Should_Have_Error_When_BranchId_Is_Empty()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.BranchId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BranchId);
    }

    /// <summary>
    /// Tests that validation fails when Items is empty.
    /// </summary>
    [Fact(DisplayName = "Should have error when Items is empty")]
    public void Should_Have_Error_When_Items_Is_Empty()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.Items = new List<CreateSaleItemRequest>();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    /// <summary>
    /// Tests that validation fails when Items is null.
    /// </summary>
    [Fact(DisplayName = "Should have error when Items is null")]
    public void Should_Have_Error_When_Items_Is_Null()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.Items = null!;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    /// <summary>
    /// Tests that validation fails when item ProductId is empty.
    /// </summary>
    [Fact(DisplayName = "Should have error when item ProductId is empty")]
    public void Should_Have_Error_When_Item_ProductId_Is_Empty()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.Items[0].ProductId = Guid.Empty;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].ProductId");
    }

    /// <summary>
    /// Tests that validation fails when item quantity is zero.
    /// </summary>
    [Fact(DisplayName = "Should have error when item quantity is zero")]
    public void Should_Have_Error_When_Item_Quantity_Is_Zero()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.Items[0].Quantity = 0;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
    }

    /// <summary>
    /// Tests that validation fails when item quantity exceeds maximum limit.
    /// </summary>
    [Fact(DisplayName = "Should have error when item quantity exceeds maximum")]
    public void Should_Have_Error_When_Item_Quantity_Exceeds_Maximum()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.Items[0].Quantity = 25; // Maximum is 20

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
    }

    /// <summary>
    /// Tests that validation passes when item quantity is at minimum limit.
    /// </summary>
    [Fact(DisplayName = "Should not have error when item quantity is at minimum")]
    public void Should_Not_Have_Error_When_Item_Quantity_Is_At_Minimum()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.Items[0].Quantity = 1; // Minimum is 1

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor("Items[0].Quantity");
    }

    /// <summary>
    /// Tests that validation passes when item quantity is at maximum limit.
    /// </summary>
    [Fact(DisplayName = "Should not have error when item quantity is at maximum")]
    public void Should_Not_Have_Error_When_Item_Quantity_Is_At_Maximum()
    {
        // Arrange
        var command = SaleCommandTestData.GenerateValidCreateSaleCommand();
        command.Items[0].Quantity = 20; // Maximum is 20

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor("Items[0].Quantity");
    }
}