using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Handler for processing RemoveSaleItemCommand requests.
/// </summary>
/// <remarks>
/// This handler orchestrates the business logic for removing items from sales.
/// It validates the request, checks business rules, manages stock returns,
/// and coordinates with domain entities to complete the operation.
/// 
/// Key responsibilities:
/// - Validate command using FluentValidation
/// - Verify sale exists and is not cancelled
/// - Verify sale item exists within the sale and is not already cancelled
/// - Return stock to product inventory
/// - Remove item using domain methods (marks as cancelled)
/// - Update and persist changes
/// - Return structured result with removal details
/// </remarks>
public class RemoveSaleItemHandler : IRequestHandler<RemoveSaleItemCommand, RemoveSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of RemoveSaleItemHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository for data access</param>
    /// <param name="productRepository">The product repository for data access</param>
    /// <param name="mapper">The AutoMapper instance for object mapping</param>
    public RemoveSaleItemHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the RemoveSaleItemCommand request.
    /// </summary>
    /// <param name="request">The RemoveSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RemoveSaleItemResult with operation details</returns>
    /// <exception cref="ValidationException">Thrown when command validation fails</exception>
    /// <exception cref="ArgumentException">Thrown when sale or item not found</exception>
    /// <exception cref="InvalidOperationException">Thrown when business rules are violated</exception>
    public async Task<RemoveSaleItemResult> Handle(RemoveSaleItemCommand request, CancellationToken cancellationToken)
    {
        // Validate the command
        var validator = new RemoveSaleItemCommandValidator();
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
            throw new InvalidOperationException("Cannot remove items from a cancelled sale.");
        }

        // Find the sale item
        var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId && !i.IsCancelled);
        if (item == null)
        {
            throw new ArgumentException($"Sale item with ID {request.ItemId} not found in sale {request.SaleId} or already cancelled.", nameof(request.ItemId));
        }

        // Get the product for stock management
        var product = await _productRepository.GetByIdAsync(item.Product.Id, cancellationToken);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {item.Product.Id} no longer exists.");
        }

        // Store item details before removal
        var itemQuantity = item.Quantity;
        var itemUnitPrice = item.UnitPrice;
        var itemTotalPrice = item.TotalPrice;
        var productId = item.Product.Id;

        // Return stock to product inventory
        product.AddStock(itemQuantity);
        await _productRepository.UpdateAsync(product, cancellationToken);

        // Remove item using domain method (this marks as cancelled and recalculates total)
        sale.RemoveItem(request.ItemId);

        // Save the updated sale
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Return the result
        return new RemoveSaleItemResult
        {
            ItemId = request.ItemId,
            SaleId = sale.Id,
            ProductId = productId,
            RemovedQuantity = itemQuantity,
            UnitPrice = itemUnitPrice,
            RemovedTotalPrice = itemTotalPrice,
            SaleTotalAmount = sale.TotalAmount,
            IsCancelled = true
        };
    }
}