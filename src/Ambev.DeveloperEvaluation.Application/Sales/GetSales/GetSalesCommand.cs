using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Command for retrieving a paginated list of sales.
/// </summary>
/// <remarks>
/// This command is used to get a paginated list of sales with optional filtering.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="GetSalesResult"/>.
/// </remarks>
public class GetSalesCommand : IRequest<GetSalesResult>
{
    /// <summary>
    /// Gets or sets the page number (1-based).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the optional customer ID filter.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the optional branch ID filter.
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Gets or sets the optional filter for cancelled status.
    /// </summary>
    public bool? IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the optional start date filter.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the optional end date filter.
    /// </summary>
    public DateTime? EndDate { get; set; }
}