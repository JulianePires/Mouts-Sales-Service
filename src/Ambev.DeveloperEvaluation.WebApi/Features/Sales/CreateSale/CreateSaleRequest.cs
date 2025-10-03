namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the customer making the purchase.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the branch where the sale is made.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the date when the sale is made.
    /// If not provided, current date will be used.
    /// </summary>
    public DateTime? SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the list of items being sold.
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}

/// <summary>
/// Represents a sale item request for creating a sale.
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the product being sold.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being sold.
    /// </summary>
    public int Quantity { get; set; }
}