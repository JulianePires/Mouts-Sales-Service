using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Common.Extensions;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

/// <summary>
/// Handler for processing AddSaleItemCommand requests.
/// </summary>
public class AddSaleItemHandler : IRequestHandler<AddSaleItemCommand, AddSaleItemResult>
{
    private readonly ISaleService _saleService;
    private readonly IProductRepository _productRepository;
    private readonly ISaleRepository _saleRepository;

    public AddSaleItemHandler(
        ISaleService saleService,
        IProductRepository productRepository,
        ISaleRepository saleRepository)
    {
        _saleService = saleService;
        _productRepository = productRepository;
        _saleRepository = saleRepository;
    }

    public async Task<AddSaleItemResult> Handle(AddSaleItemCommand request, CancellationToken cancellationToken)
    {
        var validator = new AddSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Get entities using new utilities
        var sale = await _saleService.EnsureActiveSaleAsync(request.SaleId, cancellationToken);
        var product = await _productRepository.GetOrThrowAsync(request.ProductId, nameof(request.ProductId), cancellationToken);

        // Add item to sale (domain handles business rules)
        var newItem = sale.AddItem(product, request.Quantity, request.UnitPrice);

        product.RemoveStock(request.Quantity);
        await _productRepository.UpdateAsync(product, cancellationToken);
        await _saleRepository.UpdateAsync(sale, cancellationToken);
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