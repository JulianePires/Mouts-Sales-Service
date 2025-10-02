using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover entity creation, item management, business rules validation, discount calculations, and state management.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that a valid sale can be created successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then sale should be created successfully")]
    public void Given_ValidSaleData_When_CreatingSale_Then_SaleShouldBeCreatedSuccessfully()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var branch = BranchTestData.GenerateValidBranch();

        // Act
        var sale = Sale.Create(customer, branch, "SAL202410021238");

        // Assert
        sale.Should().NotBeNull();
        sale.Customer.Should().Be(customer);
        sale.Branch.Should().Be(branch);
        sale.SaleNumber.Should().NotBeEmpty();
        sale.SaleDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        sale.IsCancelled.Should().BeFalse();
        sale.TotalAmount.Should().Be(0);
        sale.Items.Should().BeEmpty();
        sale.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that sale creation fails with null customer.
    /// </summary>
    [Fact(DisplayName = "Given null customer When creating sale Then should throw ArgumentException")]
    public void Given_NullCustomer_When_CreatingSale_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act & Assert
        var action = () => Sale.Create(null!, branch, "SAL202410021238");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Customer cannot be null.*");
    }

    /// <summary>
    /// Tests that sale creation fails with inactive customer.
    /// </summary>
    [Fact(DisplayName = "Given inactive customer When creating sale Then should throw InvalidOperationException")]
    public void Given_InactiveCustomer_When_CreatingSale_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var inactiveCustomer = CustomerTestData.GenerateInactiveCustomer();
        var branch = BranchTestData.GenerateValidBranch();

        // Act & Assert
        var action = () => Sale.Create(inactiveCustomer, branch, "SAL202410021239");
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot create sale for inactive customer.*");
    }

    /// <summary>
    /// Tests that sale creation fails with inactive branch.
    /// </summary>
    [Fact(DisplayName = "Given inactive branch When creating sale Then should throw InvalidOperationException")]
    public void Given_InactiveBranch_When_CreatingSale_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var inactiveBranch = BranchTestData.GenerateInactiveBranch();

        // Act & Assert
        var action = () => Sale.Create(customer, inactiveBranch, "SAL202410021240");
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot create sale for inactive branch.*");
    }

    /// <summary>
    /// Tests that items can be added to sale successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale When adding item Then item should be added and total calculated")]
    public void Given_ValidSale_When_AddingItem_Then_ItemShouldBeAddedAndTotalCalculated()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        product.Price = 10.00m;
        var quantity = 5;

        // Act
        sale.AddItem(product, quantity);

        // Assert
        sale.Items.Should().HaveCount(1);
        sale.Items.First().Product.Should().Be(product);
        sale.Items.First().Quantity.Should().Be(quantity);
        sale.Items.First().DiscountPercent.Should().Be(10m); // 4-9 range
        sale.TotalAmount.Should().BeGreaterThan(0);
        sale.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that adding item to cancelled sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale When adding item Then should throw InvalidOperationException")]
    public void Given_CancelledSale_When_AddingItem_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sale = SaleTestData.GenerateCancelledSale();
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        var action = () => sale.AddItem(product, 5);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add items to a cancelled sale.*");
    }

    /// <summary>
    /// Tests that adding unavailable product throws exception.
    /// </summary>
    [Fact(DisplayName = "Given unavailable product When adding to sale Then should throw InvalidOperationException")]
    public void Given_UnavailableProduct_When_AddingToSale_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var unavailableProduct = ProductTestData.GenerateUnavailableProduct();

        // Act & Assert
        var action = () => sale.AddItem(unavailableProduct, 5);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Product is not available for sale.*");
    }

    /// <summary>
    /// Tests that adding items exceeding 20-unit limit per product throws exception.
    /// </summary>
    [Fact(DisplayName = "Given product at limit When adding more items Then should throw InvalidOperationException")]
    public void Given_ProductAtLimit_When_AddingMoreItems_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        
        sale.AddItem(product, 15);

        // Act & Assert
        var action = () => sale.AddItem(product, 10); // Total would be 25
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 units of the same product in a single sale.*");
    }

    /// <summary>
    /// Tests that items can be removed from sale successfully.
    /// </summary>
    [Fact(DisplayName = "Given sale with items When removing item Then item should be cancelled and total recalculated")]
    public void Given_SaleWithItems_When_RemovingItem_Then_ItemShouldBeCancelledAndTotalRecalculated()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        sale.AddItem(product, 5);
        var itemId = sale.Items.First().Id;
        var originalTotal = sale.TotalAmount;

        // Act
        sale.RemoveItem(itemId);

        // Assert
        sale.Items.First().IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(0);
        sale.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that item quantity can be updated successfully.
    /// </summary>
    [Fact(DisplayName = "Given sale with items When updating item quantity Then quantity should be updated and total recalculated")]
    public void Given_SaleWithItems_When_UpdatingItemQuantity_Then_QuantityShouldBeUpdatedAndTotalRecalculated()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        sale.AddItem(product, 5);
        var itemId = sale.Items.First().Id;
        var newQuantity = 8;

        // Act
        sale.UpdateItemQuantity(itemId, newQuantity);

        // Assert
        sale.Items.First().Quantity.Should().Be(newQuantity);
        sale.Items.First().DiscountPercent.Should().Be(10m); // Still in 4-9 range
        sale.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that updating item quantity beyond limit throws exception.
    /// </summary>
    [Fact(DisplayName = "Given sale with single item When updating quantity beyond limit Then should throw InvalidOperationException")]
    public void Given_SaleWithMultipleItems_When_UpdatingQuantityBeyondLimit_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        
        // Add a single item with 10 units
        sale.AddItem(product, 10);
        
        var itemId = sale.Items.First().Id;

        // Act & Assert - Try to update the item to 21 units (exceeding the 20-unit limit)
        var action = () => sale.UpdateItemQuantity(itemId, 21);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 units of the same product in a single sale.*");
    }

    /// <summary>
    /// Tests that sale can be cancelled successfully.
    /// </summary>
    [Fact(DisplayName = "Given active sale When cancelling Then sale should be cancelled")]
    public void Given_ActiveSale_When_Cancelling_Then_SaleShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        sale.AddItem(product, 5);

        // Act
        sale.Cancel();

        // Assert
        sale.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(0);
        sale.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that cancelled sale can be reactivated successfully.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale When reactivating Then sale should be active")]
    public void Given_CancelledSale_When_Reactivating_Then_SaleShouldBeActive()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        sale.AddItem(product, 5);
        var originalTotal = sale.TotalAmount;
        sale.Cancel();

        // Act
        sale.Reactivate();

        // Assert
        sale.IsCancelled.Should().BeFalse();
        sale.TotalAmount.Should().Be(originalTotal);
        sale.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that total discount calculation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given sale with mixed discounts When calculating total discount Then should return correct amount")]
    public void Given_SaleWithMixedDiscounts_When_CalculatingTotalDiscount_Then_ShouldReturnCorrectAmount()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithMixedDiscounts();

        // Act
        var totalDiscount = sale.GetTotalDiscount();

        // Assert
        totalDiscount.Should().BeGreaterThan(0);
        totalDiscount.Should().Be(sale.Items.Where(i => !i.IsCancelled).Sum(i => i.GetDiscountAmount()));
    }

    /// <summary>
    /// Tests that subtotal calculation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given sale with items When calculating subtotal Then should return correct amount")]
    public void Given_SaleWithItems_When_CalculatingSubtotal_Then_ShouldReturnCorrectAmount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        product.Price = 10.00m;
        sale.AddItem(product, 5);

        // Act
        var subtotal = sale.GetSubtotal();

        // Assert
        subtotal.Should().Be(50.00m); // 5 * 10.00
    }

    /// <summary>
    /// Tests that active item count calculation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given sale with cancelled and active items When counting active items Then should return correct count")]
    public void Given_SaleWithCancelledAndActiveItems_When_CountingActiveItems_Then_ShouldReturnCorrectCount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();
        
        sale.AddItem(product1, 5);
        sale.AddItem(product2, 3);
        
        var firstItemId = sale.Items.First().Id;
        sale.RemoveItem(firstItemId);

        // Act
        var activeCount = sale.GetActiveItemCount();

        // Assert
        activeCount.Should().Be(1);
    }

    /// <summary>
    /// Tests that HasItems works correctly.
    /// </summary>
    [Fact(DisplayName = "Given sale with active items When checking has items Then should return true")]
    public void Given_SaleWithActiveItems_When_CheckingHasItems_Then_ShouldReturnTrue()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        sale.AddItem(product, 5);

        // Act
        var hasItems = sale.HasItems();

        // Assert
        hasItems.Should().BeTrue();
    }

    /// <summary>
    /// Tests that empty sale returns false for HasItems.
    /// </summary>
    [Fact(DisplayName = "Given empty sale When checking has items Then should return false")]
    public void Given_EmptySale_When_CheckingHasItems_Then_ShouldReturnFalse()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var hasItems = sale.HasItems();

        // Assert
        hasItems.Should().BeFalse();
    }

    /// <summary>
    /// Tests that valid sale passes validation.
    /// </summary>
    [Fact(DisplayName = "Given valid sale When validating Then validation should pass")]
    public void Given_ValidSale_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that sale with empty sale number fails validation.
    /// </summary>
    [Fact(DisplayName = "Given sale with empty number When validating Then validation should fail")]
    public void Given_SaleWithEmptyNumber_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithEmptyNumber();

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Sale number"));
    }

    /// <summary>
    /// Tests that sale with future date fails validation.
    /// </summary>
    [Fact(DisplayName = "Given sale with future date When validating Then validation should fail")]
    public void Given_SaleWithFutureDate_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithFutureDate();

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Sale date"));
    }

    /// <summary>
    /// Tests that sale interface implementation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given sale When accessing via interface Then properties should be exposed correctly")]
    public void Given_Sale_When_AccessingViaInterface_Then_PropertiesShouldBeExposedCorrectly()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        sale.AddItem(product, 5);

        // Act
        var iSale = (ISale)sale;

        // Assert
        iSale.Id.Should().Be(sale.Id.ToString());
        iSale.SaleNumber.Should().Be(sale.SaleNumber);
        iSale.SaleDate.Should().Be(sale.SaleDate);
        iSale.Customer.Should().Be(sale.Customer);
        iSale.Branch.Should().Be(sale.Branch);
        iSale.Items.Should().HaveCount(sale.Items.Count);
        iSale.TotalAmount.Should().Be(sale.TotalAmount);
        iSale.IsCancelled.Should().Be(sale.IsCancelled);
        iSale.CreatedAt.Should().Be(sale.CreatedAt);
        iSale.UpdatedAt.Should().Be(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests business rule: Maximum 20 units per product across multiple items.
    /// </summary>
    [Fact(DisplayName = "Given sale with items totaling over 20 units of same product When validating Then validation should fail")]
    public void Given_SaleWithItemsTotalingOver20UnitsOfSameProduct_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = ProductTestData.GenerateValidProduct();
        
        // Manually create items that exceed the limit (bypassing the business rule check in AddItem)
        var item1 = SaleItem.Create(sale.Id, product, 12);
        var item2 = SaleItem.Create(sale.Id, product, 10);
        
        sale.Items.Add(item1);
        sale.Items.Add(item2);

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Cannot sell more than 20 units of the same product in a single sale"));
    }
}
