using NSubstitute;
using Xunit;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSale;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleHandler"/> class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly CancelSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data for testing.
    /// </summary>
    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();

        _handler = new CancelSaleHandler(_saleRepository, _productRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale cancellation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should cancel sale successfully when sale exists and is not cancelled")]
    public async Task Handle_ValidRequest_CancelsSaleSuccessfully()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = saleId;
        sale.IsCancelled = false;

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(sale.IsCancelled);
        Assert.Equal(0, sale.TotalAmount);
        Assert.NotNull(result);
        Assert.Equal(saleId, result.Id);
        Assert.True(result.IsCancelled);

        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an exception is thrown when the sale is not found.
    /// </summary>
    [Fact(DisplayName = "Handle should throw exception when sale not found")]
    public async Task Handle_SaleNotFound_ThrowsException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains($"Sale with ID {saleId} not found", exception.Message);
        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an exception is thrown when trying to cancel an already cancelled sale.
    /// </summary>
    [Fact(DisplayName = "Handle should throw exception when sale is already cancelled")]
    public async Task Handle_SaleAlreadyCancelled_ThrowsException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = saleId;
        sale.IsCancelled = true; // Already cancelled

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains($"Sale {sale.SaleNumber} is already cancelled", exception.Message);
        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an exception is thrown when sale ID is empty.
    /// </summary>
    [Fact(DisplayName = "Handle should throw exception when sale ID is empty")]
    public async Task Handle_EmptySaleId_ThrowsException()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.Empty);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Sale ID cannot be empty", exception.Message);
        await _saleRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that cancellation token is properly handled.
    /// </summary>
    [Fact(DisplayName = "Handle should handle cancellation token properly")]
    public async Task Handle_CancellationRequested_ThrowsOperationCanceledException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var cancellationToken = new CancellationToken(true);

        _saleRepository.GetByIdAsync(saleId, cancellationToken)
            .Returns(Task.FromCanceled<Sale?>(cancellationToken));

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => _handler.Handle(command, cancellationToken));
    }
}