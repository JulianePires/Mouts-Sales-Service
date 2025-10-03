using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Command for deleting a product by their ID.
/// </summary>
public class DeleteProductCommand : IRequest<DeleteProductResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to delete.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of DeleteProductCommand.
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    public DeleteProductCommand(Guid id)
    {
        Id = id;
    }
}