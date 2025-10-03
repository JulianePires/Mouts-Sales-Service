namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// API response model for CreateSale operation
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// The unique identifier of the created sale
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
    /// The unique identifier of the customer
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The unique identifier of the branch
    /// </summary>
    public Guid BranchId { get; set; }

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
    public List<SaleItemResponse> Items { get; set; } = new();
}

/// <summary>
/// Represents a sale item in the response
/// </summary>
public class SaleItemResponse
{
    /// <summary>
    /// The unique identifier of the item
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The name of the product
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity sold
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The discount applied to this item
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// The total price for this item (quantity * unit price - discount)
    /// </summary>
    public decimal TotalPrice { get; set; }
}