using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Command for removing an item from an existing sale.
/// </summary>
public class RemoveSaleItemCommand : IRequest<RemoveSaleItemResult>
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
}