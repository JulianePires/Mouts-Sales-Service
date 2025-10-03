using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Handler for processing UpdateSaleItemCommand requests.
/// </summary>
public class UpdateSaleItemHandler : IRequestHandler<UpdateSaleItemCommand, UpdateSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public UpdateSaleItemHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<UpdateSaleItemResult> Handle(UpdateSaleItemCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        if (sale == null)
        {
            throw new ArgumentException($"Sale with ID {request.SaleId} not found.", nameof(request.SaleId));
        }

        if (sale.IsCancelled)
        {
            throw new InvalidOperationException("Cannot update items in a cancelled sale.");
        }

        var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId && !i.IsCancelled);
        if (item == null)
        {
            throw new ArgumentException($"Sale item with ID {request.ItemId} not found in sale {request.SaleId}.", nameof(request.ItemId));
        }

        var product = await _productRepository.GetByIdAsync(item.Product.Id, cancellationToken);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {item.Product.Id} no longer exists.");
        }

        var currentQuantity = item.Quantity;
        var quantityDifference = request.NewQuantity - currentQuantity;
        // Domain will handle business rule validation during UpdateItemQuantity
        sale.UpdateItemQuantity(request.ItemId, request.NewQuantity, product);

        // Handle stock adjustments after domain validation
        if (quantityDifference > 0)
        {
            product.RemoveStock(quantityDifference);
        }
        else if (quantityDifference < 0)
        {
            var stockToReturn = Math.Abs(quantityDifference);
            product.AddStock(stockToReturn);
        }

        if (quantityDifference != 0)
        {
            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        await _saleRepository.UpdateAsync(sale, cancellationToken);
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