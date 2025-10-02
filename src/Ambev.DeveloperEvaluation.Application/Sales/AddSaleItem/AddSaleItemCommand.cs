using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

/// <summary>
/// Command for adding an item to an existing sale.
/// </summary>
/// <remarks>
/// This command is used to add a new product item to an existing sale transaction.
/// It includes validation for business rules such as maximum quantity per product (20 units),
/// product availability, and discount calculations based on quantity.
/// 
/// Business rules enforced:
/// - Sale must exist and not be cancelled
/// - Product must be active and available for sale
/// - Quantity must be between 1 and 20 units per product
/// - Total quantity for same product across all items cannot exceed 20
/// - Automatic discount application: 4-9 units = 10%, 10-20 units = 20%
/// </remarks>
public class AddSaleItemCommand : IRequest<AddSaleItemResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to add the item to.
    /// Must be a valid existing sale ID that is not cancelled.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product to add to the sale.
    /// Must be a valid existing product ID that is available for sale.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product to add.
    /// Must be between 1 and 20 units. Combined with existing quantities
    /// of the same product, cannot exceed 20 total units.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the optional unit price override.
    /// If not provided, uses the current product price.
    /// Used for promotional pricing or special discounts.
    /// </summary>
    public decimal? UnitPrice { get; set; }
}