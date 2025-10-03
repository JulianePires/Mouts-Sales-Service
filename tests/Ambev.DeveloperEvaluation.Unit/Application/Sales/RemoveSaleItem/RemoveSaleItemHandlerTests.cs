using FluentValidation;
using NSubstitute;
using Xunit;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using SaleTestData = Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData.SaleTestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.RemoveSaleItem;

/// <summary>
/// Contains unit tests for the <see cref="RemoveSaleItemHandler"/> class.
/// Tests all scenarios including successful item removal, validation failures,
/// business rule violations, and stock management.
/// </summary>
public class RemoveSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly RemoveSaleItemHandler _handler;

    public RemoveSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();

        _handler = new RemoveSaleItemHandler(
            _saleRepository,
            _productRepository);
    }

    /// <summary>
    /// Tests that a valid item removal request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should remove item successfully when request is valid")]
    public async Task Handle_ValidRequest_RemovesItemSuccessfully()
    {
        // Arrange
        var command = new RemoveSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid()
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 50;
        var initialStock = product.StockQuantity;

        var saleItem = SaleItem.Create(sale.Id, product, 5, 10.00m);
        saleItem.Id = command.ItemId;
        sale.Items.Clear();
        sale.Items.Add(saleItem);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
            .Returns(product);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.ItemId, result.ItemId);
        Assert.Equal(command.SaleId, result.SaleId);
        Assert.Equal(product.Id, result.ProductId);
        Assert.Equal(5, result.RemovedQuantity);
        Assert.True(result.IsCancelled);

        // Verify repositories were called
        await _productRepository.Received(1).UpdateAsync(product, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());

        // Verify stock was returned (check that product stock increased)
        Assert.Equal(55, product.StockQuantity); // 50 + 5 = 55
    }

    /// <summary>
    /// Tests that ValidationException is thrown when command is invalid.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new RemoveSaleItemCommand
        {
            SaleId = Guid.Empty, // Invalid
            ItemId = Guid.NewGuid()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests that ArgumentException is thrown when sale is not found.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ArgumentException when sale not found")]
    public async Task Handle_SaleNotFound_ThrowsArgumentException()
    {
        // Arrange
        var command = new RemoveSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid()
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Sale with ID", exception.Message);
        Assert.Contains("not found", exception.Message);
    }

    /// <summary>
    /// Tests that InvalidOperationException is thrown when sale is cancelled.
    /// </summary>
    [Fact(DisplayName = "Handle should throw InvalidOperationException when sale is cancelled")]
    public async Task Handle_CancelledSale_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new RemoveSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid()
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        sale.Status = SaleStatus.Cancelled;

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Cannot remove items from a cancelled sale", exception.Message);
    }

    /// <summary>
    /// Tests that ArgumentException is thrown when sale item is not found.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ArgumentException when item not found")]
    public async Task Handle_ItemNotFound_ThrowsArgumentException()
    {
        // Arrange
        var command = new RemoveSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid()
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        sale.Items.Clear(); // No items

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Sale item with ID", exception.Message);
        Assert.Contains("not found in sale", exception.Message);
    }

    /// <summary>
    /// Tests that ArgumentException is thrown when item is already cancelled.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ArgumentException when item already cancelled")]
    public async Task Handle_ItemAlreadyCancelled_ThrowsArgumentException()
    {
        // Arrange
        var command = new RemoveSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid()
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        var product = ProductTestData.GenerateValidProduct();

        var saleItem = SaleItem.Create(sale.Id, product, 5, 10.00m);
        saleItem.Id = command.ItemId;
        saleItem.Cancel(); // Already cancelled
        sale.Items.Clear();
        sale.Items.Add(saleItem);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("or already cancelled", exception.Message);
    }
}