using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleValidator class.
/// Tests cover validation of all sale properties including sale number, customer,
/// branch, sale date, total amount, and complex business rules for item management.
/// </summary>
public class SaleValidatorTests
{
    private readonly SaleValidator _validator;

    public SaleValidatorTests()
    {
        _validator = new SaleValidator();
    }

    /// <summary>
    /// Tests that validation passes when all sale properties are valid.
    /// </summary>
    [Fact(DisplayName = "Valid sale should pass all validation rules")]
    public void Given_ValidSale_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when sale number is empty.
    /// </summary>
    [Fact(DisplayName = "Empty sale number should fail validation")]
    public void Given_EmptySaleNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleNumber = string.Empty;

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.SaleNumber)
            .WithErrorMessage("Sale number is required.");
    }

    /// <summary>
    /// Tests that validation fails when sale number is too short.
    /// </summary>
    [Fact(DisplayName = "Sale number too short should fail validation")]
    public void Given_SaleNumberTooShort_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleNumber = "123";

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.SaleNumber)
            .WithErrorMessage("Sale number must be at least 5 characters long.");
    }

    /// <summary>
    /// Tests that validation fails when sale number is too long.
    /// </summary>
    [Fact(DisplayName = "Sale number too long should fail validation")]
    public void Given_SaleNumberTooLong_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleNumber = new string('A', 51);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.SaleNumber)
            .WithErrorMessage("Sale number cannot be longer than 50 characters.");
    }

    /// <summary>
    /// Tests that validation fails when customer is null.
    /// </summary>
    [Fact(DisplayName = "Null customer should fail validation")]
    public void Given_NullCustomer_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Customer = null!;

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Customer)
            .WithErrorMessage("Customer is required.");
    }

    /// <summary>
    /// Tests that validation fails when branch is null.
    /// </summary>
    [Fact(DisplayName = "Null branch should fail validation")]
    public void Given_NullBranch_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Branch = null!;

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Branch)
            .WithErrorMessage("Branch is required.");
    }

    /// <summary>
    /// Tests that validation fails when sale date is too far in the future.
    /// </summary>
    [Fact(DisplayName = "Sale date too far in future should fail validation")]
    public void Given_SaleDateTooFarInFuture_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleDate = DateTime.UtcNow.AddDays(2);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.SaleDate)
            .WithErrorMessage("Sale date cannot be more than 1 day in the future.");
    }

    /// <summary>
    /// Tests that validation fails when sale date is too far in the past.
    /// </summary>
    [Fact(DisplayName = "Sale date too far in past should fail validation")]
    public void Given_SaleDateTooFarInPast_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleDate = DateTime.UtcNow.AddYears(-11);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.SaleDate)
            .WithErrorMessage("Sale date cannot be more than 10 years in the past.");
    }

    /// <summary>
    /// Tests that validation fails when total amount is negative for non-cancelled sales.
    /// </summary>
    [Fact(DisplayName = "Negative total amount should fail validation")]
    public void Given_NegativeTotalAmount_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.TotalAmount = -100m;
        sale.Status = SaleStatus.Draft;

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.TotalAmount)
            .WithErrorMessage("Total amount cannot be negative.");
    }

    /// <summary>
    /// Tests that validation passes when total amount is negative for cancelled sales.
    /// </summary>
    [Fact(DisplayName = "Negative total amount for cancelled sale should pass validation")]
    public void Given_CancelledSaleWithNegativeTotalAmount_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.TotalAmount = -100m;
        sale.Status = SaleStatus.Cancelled;

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldNotHaveValidationErrorFor(s => s.TotalAmount);
    }

    /// <summary>
    /// Tests that validation fails when creation date is in the future.
    /// </summary>
    [Fact(DisplayName = "Future creation date should fail validation")]
    public void Given_FutureCreationDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.CreatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.CreatedAt)
            .WithErrorMessage("Sale creation date cannot be in the future.");
    }

    /// <summary>
    /// Tests that validation fails when customer is inactive.
    /// </summary>
    [Fact(DisplayName = "Inactive customer should fail validation")]
    public void Given_InactiveCustomer_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Customer.IsActive = false;

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Customer)
            .WithErrorMessage("Cannot create sale for inactive customer.");
    }

    /// <summary>
    /// Tests that validation fails when branch is inactive.
    /// </summary>
    [Fact(DisplayName = "Inactive branch should fail validation")]
    public void Given_InactiveBranch_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Branch.IsActive = false;

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Branch)
            .WithErrorMessage("Cannot create sale for inactive branch.");
    }

    /// <summary>
    /// Tests business rule: maximum 20 units per product across all items.
    /// </summary>
    [Fact(DisplayName = "Items exceeding 20 units per product should fail validation")]
    public void Given_ItemsExceeding20UnitsPerProduct_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();

        // Create items that together exceed 20 units for the same product
        var item1 = SaleItemTestData.GenerateValidSaleItem(sale.Id, product);
        var item2 = SaleItemTestData.GenerateValidSaleItem(sale.Id, product);

        item1.UpdateQuantity(12);
        item2.UpdateQuantity(10); // Total: 22 units

        sale.Items.Add(item1);
        sale.Items.Add(item2);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Items)
            .WithErrorMessage("Cannot sell more than 20 units of the same product in a single sale.");
    }

    /// <summary>
    /// Tests that validation passes when items total exactly 20 units per product.
    /// </summary>
    [Fact(DisplayName = "Items totaling exactly 20 units per product should pass validation")]
    public void Given_ItemsTotaling20UnitsPerProduct_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();

        // Create items that together equal exactly 20 units for the same product
        var item1 = SaleItemTestData.GenerateValidSaleItem(sale.Id, product);
        var item2 = SaleItemTestData.GenerateValidSaleItem(sale.Id, product);

        item1.UpdateQuantity(12);
        item2.UpdateQuantity(8); // Total: 20 units (at limit)

        sale.Items.Add(item1);
        sale.Items.Add(item2);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldNotHaveValidationErrorFor(s => s.Items);
    }

    /// <summary>
    /// Tests that cancelled items are excluded from the 20-unit limit calculation.
    /// </summary>
    [Fact(DisplayName = "Cancelled items should be excluded from unit limit validation")]
    public void Given_CancelledItemsExcludedFromLimit_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();

        // Create items where cancelled items would exceed limit, but active items don't
        var item1 = SaleItemTestData.GenerateValidSaleItem(sale.Id, product);
        var item2 = SaleItemTestData.GenerateValidSaleItem(sale.Id, product);

        item1.UpdateQuantity(15);
        item2.UpdateQuantity(10); // This will be cancelled, so shouldn't count
        item2.Cancel(); // Cancel after setting quantity

        sale.Items.Add(item1);
        sale.Items.Add(item2);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldNotHaveValidationErrorFor(s => s.Items);
    }

    /// <summary>
    /// Tests that individual sale item validation errors are included.
    /// </summary>
    [Fact(DisplayName = "Individual sale item validation errors should be included")]
    public void Given_SaleWithInvalidItem_When_Validated_Then_ShouldHaveItemErrors()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var invalidItem = SaleItemTestData.GenerateValidSaleItem(sale.Id);

        // Make the item invalid
        invalidItem.Quantity = 0; // Invalid quantity

        sale.Items.Add(invalidItem);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Items"));
    }

    /// <summary>
    /// Tests that validation passes for sale with valid items.
    /// </summary>
    [Fact(DisplayName = "Sale with valid items should pass validation")]
    public void Given_SaleWithValidItems_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();

        var item1 = SaleItemTestData.GenerateValidSaleItem(sale.Id, product1);
        var item2 = SaleItemTestData.GenerateValidSaleItem(sale.Id, product2);

        item1.UpdateQuantity(5);
        item2.UpdateQuantity(8);

        sale.Items.Add(item1);
        sale.Items.Add(item2);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation passes for empty sale (no items).
    /// </summary>
    [Fact(DisplayName = "Empty sale should pass validation")]
    public void Given_EmptySale_When_Validated_Then_ShouldNotHaveItemErrors()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        // No items added

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldNotHaveValidationErrorFor(s => s.Items);
    }
}
