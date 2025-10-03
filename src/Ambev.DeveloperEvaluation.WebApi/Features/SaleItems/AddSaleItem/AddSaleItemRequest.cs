namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.AddSaleItem;

/// <summary>
/// Request model for adding an item to a sale
/// </summary>
public class AddSaleItemRequest
{
    /// <summary>
    /// The unique identifier of the product to add
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The quantity of the product to add
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Optional unit price override. If not provided, product's current price will be used
    /// </summary>
    public decimal? UnitPrice { get; set; }
}