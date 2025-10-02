using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Command for updating the quantity of an existing sale item.
/// </summary>
/// <remarks>
/// This command is used to update the quantity of an existing item within a sale transaction.
/// It includes validation for business rules such as maximum quantity per product (20 units),
/// and automatic recalculation of discounts based on the new quantity.
/// 
/// Business rules enforced:
/// - Sale must exist and not be cancelled
/// - Sale item must exist within the specified sale
/// - New quantity must be between 1 and 20 units per product
/// - Total quantity for same product across all items cannot exceed 20
/// - Automatic discount recalculation: 4-9 units = 10%, 10-20 units = 20%
/// - Stock availability validation for quantity increases
/// </remarks>
public class UpdateSaleItemCommand : IRequest<UpdateSaleItemResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale containing the item to update.
    /// Must be a valid existing sale ID that is not cancelled.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the sale item to update.
    /// Must be a valid existing item ID within the specified sale.
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Gets or sets the new quantity for the sale item.
    /// Must be between 1 and 20 units. Combined with other quantities
    /// of the same product, cannot exceed 20 total units.
    /// </summary>
    public int NewQuantity { get; set; }
}