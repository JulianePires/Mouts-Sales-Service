namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.UpdateSaleItem;

/// <summary>
/// Request model for updating a sale item quantity
/// </summary>
public class UpdateSaleItemRequest
{
    /// <summary>
    /// The new quantity for the sale item
    /// </summary>
    public int Quantity { get; set; }
}