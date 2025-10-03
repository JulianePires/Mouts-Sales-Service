using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Handler for processing RemoveSaleItemCommand requests.
/// </summary>
public class RemoveSaleItemHandler : IRequestHandler<RemoveSaleItemCommand, RemoveSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public RemoveSaleItemHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<RemoveSaleItemResult> Handle(RemoveSaleItemCommand request, CancellationToken cancellationToken)
    {
        var validator = new RemoveSaleItemCommandValidator();
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

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot remove items from a cancelled sale.");
        }

        var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId && !i.IsCancelled);
        if (item == null)
        {
            throw new ArgumentException($"Sale item with ID {request.ItemId} not found in sale {request.SaleId} or already cancelled.", nameof(request.ItemId));
        }

        var product = await _productRepository.GetByIdAsync(item.Product.Id, cancellationToken);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {item.Product.Id} no longer exists.");
        }
        var itemQuantity = item.Quantity;
        var itemUnitPrice = item.UnitPrice;
        var itemTotalPrice = item.TotalPrice;
        var productId = item.Product.Id;

        product.AddStock(itemQuantity);
        await _productRepository.UpdateAsync(product, cancellationToken);

        sale.RemoveItem(request.ItemId);
        await _saleRepository.UpdateAsync(sale, cancellationToken);

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