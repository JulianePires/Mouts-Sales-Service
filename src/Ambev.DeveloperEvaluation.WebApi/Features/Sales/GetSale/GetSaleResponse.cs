namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Response model for GetSale operation
/// </summary>
public class GetSaleResponse
{
    /// <summary>
    /// The unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number for identification
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Customer information
    /// </summary>
    public CustomerInfo Customer { get; set; } = new();

    /// <summary>
    /// Branch information
    /// </summary>
    public BranchInfo Branch { get; set; } = new();

    /// <summary>
    /// The total amount of the sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The status of the sale
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// The list of items in the sale
    /// </summary>
    public List<SaleItemInfo> Items { get; set; } = new();

    /// <summary>
    /// When the sale was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the sale was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Customer information in sale response
/// </summary>
public class CustomerInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Branch information in sale response
/// </summary>
public class BranchInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Sale item information in sale response
/// </summary>
public class SaleItemInfo
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}