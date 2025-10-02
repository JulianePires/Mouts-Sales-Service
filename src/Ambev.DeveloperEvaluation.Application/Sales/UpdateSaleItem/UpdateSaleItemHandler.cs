using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Handler for processing UpdateSaleItemCommand requests.
/// </summary>
/// <remarks>
/// This handler orchestrates the business logic for updating sale item quantities.
/// It validates the request, checks business rules, manages stock adjustments,
/// and coordinates with domain entities to complete the operation.
/// 
/// Key responsibilities:
/// - Validate command using FluentValidation
/// - Verify sale exists and is not cancelled
/// - Verify sale item exists within the sale
/// - Check quantity limits and business rules
/// - Manage stock adjustments (increase/decrease)
/// - Update item quantity using domain methods
/// - Update and persist changes
/// - Return structured result with quantity difference
/// </remarks>
public class UpdateSaleItemHandler : IRequestHandler<UpdateSaleItemCommand, UpdateSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateSaleItemHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository for data access</param>
    /// <param name="productRepository">The product repository for data access</param>
    /// <param name="mapper">The AutoMapper instance for object mapping</param>
    public UpdateSaleItemHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateSaleItemCommand request.
    /// </summary>
    /// <param name="request">The UpdateSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>UpdateSaleItemResult with operation details</returns>
    /// <exception cref="ValidationException">Thrown when command validation fails</exception>
    /// <exception cref="ArgumentException">Thrown when sale or item not found</exception>
    /// <exception cref="InvalidOperationException">Thrown when business rules are violated</exception>
    public async Task<UpdateSaleItemResult> Handle(UpdateSaleItemCommand request, CancellationToken cancellationToken)
    {
        // Validate the command
        var validator = new UpdateSaleItemCommandValidator();
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
            throw new InvalidOperationException("Cannot update items in a cancelled sale.");
        }

        // Find the sale item
        var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId && !i.IsCancelled);
        if (item == null)
        {
            throw new ArgumentException($"Sale item with ID {request.ItemId} not found in sale {request.SaleId}.", nameof(request.ItemId));
        }

        // Get the product for stock management
        var product = await _productRepository.GetByIdAsync(item.Product.Id, cancellationToken);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {item.Product.Id} no longer exists.");
        }

        // Calculate quantity difference
        var currentQuantity = item.Quantity;
        var quantityDifference = request.NewQuantity - currentQuantity;

        // Handle stock adjustments
        if (quantityDifference > 0)
        {
            // Increasing quantity - check stock availability
            if (product.StockQuantity < quantityDifference)
            {
                throw new InvalidOperationException($"Product {product.Name} does not have sufficient stock. Available: {product.StockQuantity}, Required: {quantityDifference}");
            }
            product.RemoveStock(quantityDifference);
        }
        else if (quantityDifference < 0)
        {
            // Decreasing quantity - return stock
            var stockToReturn = Math.Abs(quantityDifference);
            product.AddStock(stockToReturn);
        }

        // Update item quantity using domain method (this handles business rules and discount recalculation)
        sale.UpdateItemQuantity(request.ItemId, request.NewQuantity);

        // Update product if stock changed
        if (quantityDifference != 0)
        {
            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        // Save the updated sale
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Get the updated item
        var updatedItem = sale.Items.FirstOrDefault(i => i.Id == request.ItemId);

        // Return the result
        return new UpdateSaleItemResult
        {
            ItemId = updatedItem!.Id,
            SaleId = sale.Id,
            ProductId = product.Id,
            Quantity = updatedItem.Quantity,
            UnitPrice = updatedItem.UnitPrice,
            DiscountPercent = updatedItem.DiscountPercent,
            TotalPrice = updatedItem.TotalPrice,
            SaleTotalAmount = sale.TotalAmount,
            QuantityDifference = quantityDifference
        };
    }
}