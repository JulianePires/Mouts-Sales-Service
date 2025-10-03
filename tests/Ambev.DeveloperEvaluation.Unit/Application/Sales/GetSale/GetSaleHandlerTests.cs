using NSubstitute;
using Xunit;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using SaleCommandTestData = Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData.SaleTestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data for testing.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();

        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale retrieval request returns the sale successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should return sale when sale exists")]
    public async Task Handle_ExistingSale_ReturnsSale()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand(saleId);
        var sale = SaleTestData.GenerateValidSale();
        sale.Id = saleId;

        var expectedResult = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            Customer = new GetSaleCustomerInfo
            {
                Id = sale.Customer.Id,
                Name = sale.Customer.Name,
                Email = sale.Customer.Email
            },
            Branch = new GetSaleBranchInfo
            {
                Id = sale.Branch.Id,
                Name = sale.Branch.Name
            },
            Items = sale.Items.Select(i => new GetSaleItemResult
            {
                Id = i.Id,
                Product = new GetSaleProductInfo
                {
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Description = i.Product.Description
                },
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                DiscountPercent = i.DiscountPercent,
                TotalPrice = i.TotalPrice,
                IsCancelled = i.IsCancelled
            }).ToList(),
            TotalAmount = sale.TotalAmount,
            IsCancelled = sale.IsCancelled
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Id, result.Id);
        Assert.Equal(expectedResult.SaleNumber, result.SaleNumber);
        Assert.Equal(expectedResult.Customer.Name, result.Customer.Name);
        Assert.Equal(expectedResult.Branch.Name, result.Branch.Name);
        Assert.Equal(expectedResult.Items.Count, result.Items.Count);
        Assert.Equal(expectedResult.TotalAmount, result.TotalAmount);
        Assert.Equal(expectedResult.IsCancelled, result.IsCancelled);

        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetSaleResult>(sale);
    }

    /// <summary>
    /// Tests that an exception is thrown when the sale is not found.
    /// </summary>
    [Fact(DisplayName = "Handle should throw exception when sale not found")]
    public async Task Handle_SaleNotFound_ThrowsException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("not found", exception.Message);
        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetSaleResult>(Arg.Any<Sale>());
    }

    /// <summary>
    /// Tests that repository is called with correct parameters.
    /// </summary>
    [Fact(DisplayName = "Handle should call repository with correct parameters")]
    public async Task Handle_ValidRequest_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand(saleId);
        var sale = SaleTestData.GenerateValidSale();
        var expectedResult = new GetSaleResult();

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).GetByIdAsync(
            Arg.Is<Guid>(x => x == saleId),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that mapper is called with correct parameters.
    /// </summary>
    [Fact(DisplayName = "Handle should call mapper with correct parameters")]
    public async Task Handle_ValidRequest_CallsMapperWithCorrectParameters()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand(saleId);
        var sale = SaleTestData.GenerateValidSale();
        var expectedResult = new GetSaleResult();

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<GetSaleResult>(Arg.Is<Sale>(x => x == sale));
    }

    /// <summary>
    /// Tests that cancellation token is properly handled.
    /// </summary>
    [Fact(DisplayName = "Handle should handle cancellation token properly")]
    public async Task Handle_CancellationRequested_ThrowsOperationCanceledException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand(saleId);
        var cancellationToken = new CancellationToken(true);

        _saleRepository.GetByIdAsync(saleId, cancellationToken)
            .Returns(Task.FromCanceled<Sale?>(cancellationToken));

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => _handler.Handle(command, cancellationToken));
    }
}