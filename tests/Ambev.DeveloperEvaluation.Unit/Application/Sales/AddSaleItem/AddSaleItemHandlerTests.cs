using FluentValidation;
using NSubstitute;
using Xunit;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using SaleTestData = Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData.SaleTestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.AddSaleItem;

/// <summary>
/// Contains unit tests for the <see cref="AddSaleItemHandler"/> class.
/// Tests all scenarios including successful item addition, validation failures,
/// business rule violations, and stock management.
/// </summary>
public class AddSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly AddSaleItemHandler _handler;

    /// <summary>
    /// Initializes a new instance of AddSaleItemHandlerTests.
    /// Sets up test dependencies and creates the handler instance.
    /// </summary>
    public AddSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();

        _handler = new AddSaleItemHandler(
            _saleRepository,
            _productRepository,
            _mapper);
    }

    /// <summary>
    /// Tests that a valid item addition request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should add item successfully when request is valid")]
    public async Task Handle_ValidRequest_AddsItemSuccessfully()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5,
            UnitPrice = 10.00m
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        sale.Items.Clear(); // Start with empty items

        var product = ProductTestData.GenerateValidProduct();
        product.Id = command.ProductId;
        product.StockQuantity = 100;
        product.Price = 10.00m;

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns(product);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.SaleId, result.SaleId);
        Assert.Equal(command.ProductId, result.ProductId);
        Assert.Equal(command.Quantity, result.Quantity);
        Assert.Equal(command.UnitPrice.Value, result.UnitPrice);

        await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that validation exception is thrown when command is invalid.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.Empty, // Invalid
            ProductId = Guid.NewGuid(),
            Quantity = 5
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
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5
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
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        sale.IsCancelled = true;

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Cannot add items to a cancelled sale", exception.Message);
    }

    /// <summary>
    /// Tests that ArgumentException is thrown when product is not found.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ArgumentException when product not found")]
    public async Task Handle_ProductNotFound_ThrowsArgumentException()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Product with ID", exception.Message);
        Assert.Contains("not found", exception.Message);
    }

    /// <summary>
    /// Tests that InvalidOperationException is thrown when product has insufficient stock.
    /// </summary>
    [Fact(DisplayName = "Handle should throw InvalidOperationException when product has insufficient stock")]
    public async Task Handle_InsufficientStock_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 10
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;

        var product = ProductTestData.GenerateValidProduct();
        product.Id = command.ProductId;
        product.StockQuantity = 5; // Less than requested quantity

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns(product);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("not available in the requested quantity", exception.Message);
    }

    /// <summary>
    /// Tests that discount is calculated correctly for qualifying quantities.
    /// </summary>
    [Fact(DisplayName = "Handle should calculate discount correctly for qualifying quantities")]
    public async Task Handle_QualifyingQuantity_CalculatesDiscountCorrectly()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 10 // Should get 20% discount
        };

        var sale = SaleTestData.GenerateValidSale();
        sale.Id = command.SaleId;
        sale.Items.Clear();

        var product = ProductTestData.GenerateValidProduct();
        product.Id = command.ProductId;
        product.StockQuantity = 100;
        product.Price = 10.00m;

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns(product);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(20m, result.DiscountPercent); // 10+ units should get 20% discount
        Assert.True(result.TotalPrice < (command.Quantity * product.Price)); // Should be discounted
    }
}