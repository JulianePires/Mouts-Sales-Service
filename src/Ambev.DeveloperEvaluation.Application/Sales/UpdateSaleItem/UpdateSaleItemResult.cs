namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Response model for UpdateSaleItem operation.
/// </summary>
public class UpdateSaleItemResult
{
    public Guid ItemId { get; set; }
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal SaleTotalAmount { get; set; }
    public int QuantityDifference { get; set; }
}