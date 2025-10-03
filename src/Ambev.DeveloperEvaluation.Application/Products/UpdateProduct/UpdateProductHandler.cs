using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Handler for processing UpdateProductCommand requests
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateProductCommand request
    /// </summary>
    /// <param name="command">The UpdateProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product details</returns>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {command.Id} not found");

        if (!string.IsNullOrEmpty(command.Name) && command.Name != product.Name)
        {
            var existingProduct = await _productRepository.GetByNameAsync(command.Name, cancellationToken);
            if (existingProduct != null)
                throw new InvalidOperationException($"Product with name {command.Name} already exists");
        }

        product.UpdateInfo(command.Name, command.Description, command.Category, command.Image);

        if (command.Price.HasValue)
            product.UpdatePrice(command.Price.Value);

        if (command.StockQuantity.HasValue)
            product.AddStock(command.StockQuantity.Value - product.StockQuantity);

        if (command.IsActive.HasValue)
        {
            if (command.IsActive.Value)
                product.Activate();
            else
                product.Deactivate();
        }

        var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);
        return _mapper.Map<UpdateProductResult>(updatedProduct);
    }
}