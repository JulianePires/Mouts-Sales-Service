namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ConfirmSale;

/// <summary>
/// Request model for confirming a sale
/// </summary>
public class ConfirmSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to confirm
    /// </summary>
    public Guid Id { get; set; }
}