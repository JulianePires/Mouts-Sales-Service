using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Command for cancelling an existing sale.
/// </summary>
/// <remarks>
/// This command is used to cancel a sale and revert inventory changes.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CancelSaleResult"/>.
/// 
/// When a sale is cancelled, the following actions are performed:
/// - Sale and all its items are marked as cancelled
/// - Product stock is restored for all items in the sale
/// - Cancellation timestamp is recorded
/// </remarks>
public class CancelSaleCommand : IRequest<CancelSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to cancel.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the reason for cancellation (optional).
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Initializes a new instance of CancelSaleCommand
    /// </summary>
    /// <param name="id">The unique identifier of the sale to cancel</param>
    /// <param name="cancellationReason">The reason for cancellation</param>
    public CancelSaleCommand(Guid id, string? cancellationReason = null)
    {
        Id = id;
        CancellationReason = cancellationReason;
    }
}