using NSubstitute;
using Xunit;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using SaleCommandTestData = Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData.SaleTestData;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSales;

/// <summary>
/// Contains unit tests for the <see cref="GetSalesHandler"/> class.
/// </summary>
public class GetSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSalesHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSalesHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data for testing.
    /// </summary>
    public GetSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        
        _handler = new GetSalesHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid get sales request returns paginated results successfully.
    /// </summary>
    [Fact(DisplayName = "Handle should return paginated sales when request is valid")]
    public async Task Handle_ValidRequest_ReturnsPaginatedSales()
    {
        // Arrange
        var command = new GetSalesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var totalSales = 25;
        var sales = SaleTestData.GenerateValidSales(10);
        var mappedSales = sales.Select(s => new GetSalesItem
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            SaleDate = s.SaleDate,
            CustomerName = s.Customer.Name,
            BranchName = s.Branch.Name,
            TotalAmount = s.TotalAmount,
            ItemCount = s.Items.Count,
            IsCancelled = s.IsCancelled,
            CreatedAt = s.CreatedAt
        }).ToList();

        _saleRepository.GetCountAsync(Arg.Any<CancellationToken>())
            .Returns(totalSales);

        _saleRepository.GetPaginatedAsync(
            command.PageNumber,
            command.PageSize,
            Arg.Any<CancellationToken>())
            .Returns(sales);

        var expectedResult = new GetSalesResult
        {
            Sales = mappedSales,
            PageNumber = command.PageNumber,
            PageSize = command.PageSize,
            TotalCount = totalSales,
            TotalPages = (int)Math.Ceiling((double)totalSales / command.PageSize),
            HasPrevious = command.PageNumber > 1,
            HasNext = command.PageNumber < (int)Math.Ceiling((double)totalSales / command.PageSize)
        };

        _mapper.Map<GetSalesResult>(Arg.Any<object>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Sales);
        Assert.Equal(expectedResult.PageNumber, result.PageNumber);
        Assert.Equal(expectedResult.PageSize, result.PageSize);
        Assert.Equal(expectedResult.TotalCount, result.TotalCount);
        Assert.Equal(expectedResult.TotalPages, result.TotalPages);

        await _saleRepository.Received(1).GetCountAsync(Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).GetPaginatedAsync(
            command.PageNumber,
            command.PageSize,
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that empty result set is handled correctly.
    /// </summary>
    [Fact(DisplayName = "Handle should return empty result when no sales found")]
    public async Task Handle_NoSalesFound_ReturnsEmptyResult()
    {
        // Arrange
        var command = new GetSalesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var emptySales = new List<Sale>();
        var emptyMappedSales = new List<GetSalesItem>();

        _saleRepository.GetCountAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        _saleRepository.GetPaginatedAsync(
            command.PageNumber,
            command.PageSize,
            Arg.Any<CancellationToken>())
            .Returns(emptySales);

        var expectedResult = new GetSalesResult
        {
            Sales = emptyMappedSales,
            PageNumber = command.PageNumber,
            PageSize = command.PageSize,
            TotalCount = 0,
            TotalPages = 0,
            HasPrevious = false,
            HasNext = false
        };

        _mapper.Map<GetSalesResult>(Arg.Any<object>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Sales);
        Assert.Empty(result.Sales);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(command.PageNumber, result.PageNumber);
        Assert.Equal(command.PageSize, result.PageSize);
    }

    /// <summary>
    /// Tests that cancellation token is properly handled.
    /// </summary>
    [Fact(DisplayName = "Handle should handle cancellation token properly")]
    public async Task Handle_CancellationRequested_ThrowsOperationCanceledException()
    {
        // Arrange
        var command = new GetSalesCommand { PageNumber = 1, PageSize = 10 };
        var cancellationToken = new CancellationToken(true);

        _saleRepository.GetCountAsync(cancellationToken)
            .Returns(Task.FromCanceled<int>(cancellationToken));

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => _handler.Handle(command, cancellationToken));
    }
}