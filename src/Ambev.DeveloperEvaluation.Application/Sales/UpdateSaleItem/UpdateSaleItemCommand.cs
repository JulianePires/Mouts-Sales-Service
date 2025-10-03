using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Command for updating the quantity of an existing sale item.
/// </summary>
public class UpdateSaleItemCommand : IRequest<UpdateSaleItemResult>
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public int NewQuantity { get; set; }
}