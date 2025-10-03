using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Command for retrieving a product by their ID.
/// </summary>
public class GetProductCommand : IRequest<GetProductResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to retrieve.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of GetProductCommand.
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    public GetProductCommand(Guid id)
    {
        Id = id;
    }
}