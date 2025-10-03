using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

/// <summary>
/// Command for confirming a sale transaction.
/// This command moves a sale from Draft status to Confirmed status after validation.
/// </summary>
public class ConfirmSaleCommand : IRequest<ConfirmSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to confirm.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of ConfirmSaleCommand.
    /// </summary>
    /// <param name="id">The sale ID to confirm</param>
    public ConfirmSaleCommand(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Parameterless constructor for model binding.
    /// </summary>
    public ConfirmSaleCommand() { }
}