using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesCommand requests
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSalesCommand request
    /// </summary>
    /// <param name="request">The GetSales command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated sales list</returns>
    public async Task<GetSalesResult> Handle(GetSalesCommand request, CancellationToken cancellationToken)
    {
        // Validate pagination parameters
        if (request.PageNumber <= 0)
            throw new ArgumentException("Page number must be greater than 0.", nameof(request.PageNumber));
        
        if (request.PageSize <= 0 || request.PageSize > 100)
            throw new ArgumentException("Page size must be between 1 and 100.", nameof(request.PageSize));

        // Get total count for pagination
        var totalCount = await _saleRepository.GetCountAsync(cancellationToken);
        
        // Calculate pagination metadata
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
        var hasPrevious = request.PageNumber > 1;
        var hasNext = request.PageNumber < totalPages;

        // Get paginated sales
        var sales = await _saleRepository.GetPaginatedAsync(request.PageNumber, request.PageSize, cancellationToken);
        
        // Filter sales if necessary (note: this could be optimized by implementing filtering in the repository)
        if (request.CustomerId.HasValue)
            sales = sales.Where(s => s.CustomerId == request.CustomerId.Value);
            
        if (request.BranchId.HasValue)
            sales = sales.Where(s => s.BranchId == request.BranchId.Value);
            
        if (request.IsCancelled.HasValue)
            sales = sales.Where(s => s.IsCancelled == request.IsCancelled.Value);
            
        if (request.StartDate.HasValue)
            sales = sales.Where(s => s.SaleDate >= request.StartDate.Value);
            
        if (request.EndDate.HasValue)
            sales = sales.Where(s => s.SaleDate <= request.EndDate.Value);

        var salesItems = sales.Select(sale => new GetSalesItem
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerName = sale.Customer?.Name ?? "Unknown",
            BranchName = sale.Branch?.Name ?? "Unknown",
            TotalAmount = sale.TotalAmount,
            ItemCount = sale.Items?.Count ?? 0,
            IsCancelled = sale.IsCancelled,
            CreatedAt = sale.CreatedAt
        }).ToList();

        return new GetSalesResult
        {
            Sales = salesItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPrevious = hasPrevious,
            HasNext = hasNext
        };
    }
}