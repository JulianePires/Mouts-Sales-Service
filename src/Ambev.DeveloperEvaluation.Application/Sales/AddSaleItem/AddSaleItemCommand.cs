using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

/// <summary>
/// Command for adding an item to an existing sale.
/// </summary>
public class AddSaleItemCommand : IRequest<AddSaleItemResult>
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}