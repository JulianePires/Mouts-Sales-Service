using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

/// <summary>
/// Handler for processing AddSaleItemCommand requests.
/// </summary>
public class AddSaleItemHandler : IRequestHandler<AddSaleItemCommand, AddSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public AddSaleItemHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<AddSaleItemResult> Handle(AddSaleItemCommand request, CancellationToken cancellationToken)
    {
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

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            throw new ArgumentException($"Product with ID {request.ProductId} not found.", nameof(request.ProductId));
        }

        // Add item to sale (domain handles business rules)
        sale.AddItem(product, request.Quantity, request.UnitPrice);

        product.RemoveStock(request.Quantity);
        await _productRepository.UpdateAsync(product, cancellationToken);
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        var newItem = sale.Items.Last();
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