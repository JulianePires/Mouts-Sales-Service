using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Command for deleting a customer by their ID.
/// </summary>
public class DeleteCustomerCommand : IRequest<DeleteCustomerResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the customer to delete.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of DeleteCustomerCommand.
    /// </summary>
    /// <param name="id">The unique identifier of the customer</param>
    public DeleteCustomerCommand(Guid id)
    {
        Id = id;
    }
}