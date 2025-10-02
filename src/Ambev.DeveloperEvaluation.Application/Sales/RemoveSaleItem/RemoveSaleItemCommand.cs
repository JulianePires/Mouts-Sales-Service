using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Command for removing an item from an existing sale.
/// </summary>
/// <remarks>
/// This command is used to remove (cancel) an existing item from a sale transaction.
/// The item is not physically deleted but marked as cancelled to maintain audit trail.
/// Stock is automatically returned to inventory when items are removed.
/// 
/// Business rules enforced:
/// - Sale must exist and not be cancelled
/// - Sale item must exist within the specified sale and not already cancelled
/// - Stock is returned to product inventory
/// - Sale total is automatically recalculated
/// - Removal maintains data integrity and audit trail
/// </remarks>
public class RemoveSaleItemCommand : IRequest<RemoveSaleItemResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale containing the item to remove.
    /// Must be a valid existing sale ID that is not cancelled.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the sale item to remove.
    /// Must be a valid existing item ID within the specified sale that is not already cancelled.
    /// </summary>
    public Guid ItemId { get; set; }
}