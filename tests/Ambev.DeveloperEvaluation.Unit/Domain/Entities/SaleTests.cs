using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover status changes and validation scenarios.
/// </summary>
public class SaleTests
{
  /// <summary>
  /// Tests basic Sale class generation
  /// </summary>
  [Fact(DisplayName = "Sale create should generate valid sale number")]
  public void Sale_Create_Should_Generate_Valid_SaleNumber()
  {
    //Arrange
    var testSale = SaleTestData.GenerateValidSale();

    //Act
    var sale = Sale.Create(testSale.CustomerId, testSale.CustomerName, testSale.BranchId, testSale.BranchName);

    //Assert
    Assert.Matches(@"^SALE\d{4}$", sale.SaleNumber);
    Assert.Equal(testSale.CustomerId, sale.CustomerId);
    Assert.Equal(testSale.CustomerName, sale.CustomerName);
    Assert.Equal(testSale.BranchId, sale.BranchId);
    Assert.Equal(testSale.BranchName, sale.BranchName);
    Assert.Equal(0, sale.TotalAmount);
  }
}