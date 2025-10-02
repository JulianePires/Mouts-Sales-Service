namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Represents the result of creating a sale.
/// </summary>
/// <remarks>
/// This class encapsulates the details of a successfully created sale, 
/// including the sale ID, sale number, total amount, and other relevant information
/// returned after the creation process is completed.
/// </remarks>
public class CreateSaleResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the created sale.
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
    /// Gets or sets the customer ID associated with the sale.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID where the sale was made.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the total amount of the sale after discounts.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the list of items in the sale.
    /// </summary>
    public List<CreateSaleItemResult> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Represents a sale item in the create sale result.
/// </summary>
public class CreateSaleItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name for display purposes.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price at the time of sale.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied.
    /// </summary>
    public decimal DiscountPercent { get; set; }

    /// <summary>
    /// Gets or sets the total price after discount.
    /// </summary>
    public decimal TotalPrice { get; set; }
}