namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Represents the result of retrieving a sale.
/// </summary>
/// <remarks>
/// This class encapsulates the details of a sale retrieved from the system,
/// including all sale information, items, customer and branch details.
/// </remarks>
public class GetSaleResult
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
    /// Gets or sets the customer information associated with the sale.
    /// </summary>
    public GetSaleCustomerInfo Customer { get; set; } = new();

    /// <summary>
    /// Gets or sets the branch information where the sale was made.
    /// </summary>
    public GetSaleBranchInfo Branch { get; set; } = new();

    /// <summary>
    /// Gets or sets the total amount of the sale after discounts.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the cancellation status of the sale.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the list of items in the sale.
    /// </summary>
    public List<GetSaleItemResult> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the sale was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Represents customer information in the sale result.
/// </summary>
public class GetSaleCustomerInfo
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer email.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Represents branch information in the sale result.
/// </summary>
public class GetSaleBranchInfo
{
    /// <summary>
    /// Gets or sets the branch ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch address.
    /// </summary>
    public string Address { get; set; } = string.Empty;
}

/// <summary>
/// Represents a sale item in the get sale result.
/// </summary>
public class GetSaleItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product information.
    /// </summary>
    public GetSaleProductInfo Product { get; set; } = new();

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

    /// <summary>
    /// Gets or sets whether the item is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Represents product information in the sale item result.
/// </summary>
public class GetSaleProductInfo
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}