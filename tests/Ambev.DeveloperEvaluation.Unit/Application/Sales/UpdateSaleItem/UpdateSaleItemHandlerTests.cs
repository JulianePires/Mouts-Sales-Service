using FluentValidation;
using NSubstitute;
using Xunit;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using SaleTestData = Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData.SaleTestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSaleItem;

/// <summary>
/// Contains unit tests for the <see cref="UpdateSaleItemHandler"/> class.
/// Tests all scenarios including successful quantity updates, validation failures,
/// business rule violations, and stock management.
/// </summary>
public class UpdateSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly UpdateSaleItemHandler _handler;

    public UpdateSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();

        _handler = new UpdateSaleItemHandler(
            _saleRepository,
            _productRepository);
    }

    /// <summary>
    /// Tests that a valid quantity update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should update item quantity successfully when request is valid")]
    public async Task Handle_ValidRequest_UpdatesQuantitySuccessfully()
    {
        // Arrange
        var command = new UpdateSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid(),
            NewQuantity = 8
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 100;

        // Create a sale item
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
        Assert.Equal(command.NewQuantity, result.Quantity);
        Assert.Equal(3, result.QuantityDifference); // 8 - 5 = 3
    }

    /// <summary>
    /// Tests that ValidationException is thrown when command is invalid.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateSaleItemCommand
        {
            SaleId = Guid.Empty, // Invalid
            ItemId = Guid.NewGuid(),
            NewQuantity = 5
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests that ArgumentException is thrown when sale item is not found.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ArgumentException when item not found")]
    public async Task Handle_ItemNotFound_ThrowsArgumentException()
    {
        // Arrange
        var command = new UpdateSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid(),
            NewQuantity = 8
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
    /// Tests that InvalidOperationException is thrown when insufficient stock for quantity increase.
    /// </summary>
    [Fact(DisplayName = "Handle should throw InvalidOperationException when insufficient stock")]
    public async Task Handle_InsufficientStock_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new UpdateSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid(),
            NewQuantity = 15 // Increasing from 5 to 15 (need 10 more)
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 5; // Only 5 available, but need 10

        var saleItem = SaleItem.Create(sale.Id, product, 5, 10.00m);
        saleItem.Id = command.ItemId;
        sale.Items.Clear();
        sale.Items.Add(saleItem);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
            .Returns(product);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("does not have sufficient stock", exception.Message);
    }

    /// <summary>
    /// Tests that stock is correctly returned when quantity is decreased.
    /// </summary>
    [Fact(DisplayName = "Handle should return stock when quantity is decreased")]
    public async Task Handle_DecreaseQuantity_ReturnsStock()
    {
        // Arrange
        var command = new UpdateSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ItemId = Guid.NewGuid(),
            NewQuantity = 3 // Decreasing from 5 to 3
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = 100;
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
        Assert.Equal(-2, result.QuantityDifference); // 3 - 5 = -2
        // Verify product repository was called to update product
        await _productRepository.Received(1).UpdateAsync(product, Arg.Any<CancellationToken>());
        // Verify stock was returned (should be 102 = 100 + 2)
        Assert.Equal(102, product.StockQuantity);
    }
}