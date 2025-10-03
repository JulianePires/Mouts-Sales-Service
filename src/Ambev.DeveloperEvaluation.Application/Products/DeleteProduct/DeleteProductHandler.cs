using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Handler for processing DeleteProductCommand requests
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of DeleteProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the DeleteProductCommand request
    /// </summary>
    /// <param name="command">The DeleteProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the deletion operation</returns>
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == Guid.Empty)
            throw new ValidationException("Product ID cannot be empty");

        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {command.Id} not found");

        var success = await _productRepository.DeleteAsync(command.Id, cancellationToken);

        return new DeleteProductResult
        {
            Success = success,
            Id = command.Id,
            Message = success ? "Product deleted successfully" : "Failed to delete product"
        };
    }
}