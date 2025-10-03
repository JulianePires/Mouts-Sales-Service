namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Represents the result of retrieving a paginated list of sales.
/// </summary>
/// <remarks>
/// This class encapsulates the paginated sales data along with pagination metadata.
/// </remarks>
public class GetSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales in the current page.
    /// </summary>
    public List<GetSalesItem> Sales { get; set; } = new();

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of sales across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets whether there is a previous page.
    /// </summary>
    public bool HasPrevious { get; set; }

    /// <summary>
    /// Gets or sets whether there is a next page.
    /// </summary>
    public bool HasNext { get; set; }
}

/// <summary>
/// Represents a sale item in the paginated list.
/// </summary>
public class GetSalesItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number for identification and tracking.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the number of items in the sale.
    /// </summary>
    public int ItemCount { get; set; }

    /// <summary>
    /// Gets or sets the cancellation status of the sale.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}