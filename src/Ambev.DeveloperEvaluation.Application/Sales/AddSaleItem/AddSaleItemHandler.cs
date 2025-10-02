using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

/// <summary>
/// Handler for processing AddSaleItemCommand requests.
/// </summary>
/// <remarks>
/// This handler orchestrates the business logic for adding items to existing sales.
/// It validates the request, checks business rules, and coordinates with domain entities
/// and repositories to complete the operation.
/// 
/// Key responsibilities:
/// - Validate command using FluentValidation
/// - Verify sale exists and is not cancelled
/// - Verify product exists and is available for sale
/// - Check quantity limits and business rules
/// - Add item to sale using domain methods
/// - Update and persist changes
/// - Return structured result
/// </remarks>
public class AddSaleItemHandler : IRequestHandler<AddSaleItemCommand, AddSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of AddSaleItemHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository for data access</param>
    /// <param name="productRepository">The product repository for data access</param>
    /// <param name="mapper">The AutoMapper instance for object mapping</param>
    public AddSaleItemHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the AddSaleItemCommand request.
    /// </summary>
    /// <param name="request">The AddSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>AddSaleItemResult with operation details</returns>
    /// <exception cref="ValidationException">Thrown when command validation fails</exception>
    /// <exception cref="ArgumentException">Thrown when sale or product not found</exception>
    /// <exception cref="InvalidOperationException">Thrown when business rules are violated</exception>
    public async Task<AddSaleItemResult> Handle(AddSaleItemCommand request, CancellationToken cancellationToken)
    {
        // Validate the command
        var validator = new AddSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Get the sale
        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        if (sale == null)
        {
            throw new ArgumentException($"Sale with ID {request.SaleId} not found.", nameof(request.SaleId));
        }

        if (sale.IsCancelled)
        {
            throw new InvalidOperationException("Cannot add items to a cancelled sale.");
        }

        // Get the product
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            throw new ArgumentException($"Product with ID {request.ProductId} not found.", nameof(request.ProductId));
        }

        // Check if product has sufficient stock
        if (product.StockQuantity < request.Quantity)
        {
            throw new InvalidOperationException($"Product {product.Name} is not available in the requested quantity ({request.Quantity}). Available: {product.StockQuantity}");
        }

        // Add item to sale using domain method (this handles business rules)
        sale.AddItem(product, request.Quantity, request.UnitPrice);

        // Update product stock
        product.RemoveStock(request.Quantity);
        await _productRepository.UpdateAsync(product, cancellationToken);

        // Save the updated sale
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Get the newly added item (last item in the collection)
        var newItem = sale.Items.Last();

        // Return the result
        return new AddSaleItemResult
        {
            ItemId = newItem.Id,
            SaleId = sale.Id,
            ProductId = product.Id,
            Quantity = newItem.Quantity,
            UnitPrice = newItem.UnitPrice,
            DiscountPercent = newItem.DiscountPercent,
            TotalPrice = newItem.TotalPrice,
            SaleTotalAmount = sale.TotalAmount
        };
    }
}