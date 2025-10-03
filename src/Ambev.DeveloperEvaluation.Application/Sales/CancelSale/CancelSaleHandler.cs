using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand requests
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CancelSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CancelSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CancelSaleCommand request
    /// </summary>
    /// <param name="request">The CancelSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cancellation result</returns>
    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ArgumentException("Sale ID cannot be empty.", nameof(request.Id));

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found.");

        if (sale.Status == SaleStatus.Cancelled)
            throw new InvalidOperationException($"Sale {sale.SaleNumber} is already cancelled.");

        // Cancel the sale using domain method
        sale.Cancel();

        var cancelledItemsCount = 0;

        // Process each item to restore inventory and mark as cancelled
        foreach (var item in sale.Items.Where(i => !i.IsCancelled))
        {
            // Restore product stock
            await _productRepository.UpdateStockAsync(
                item.Product.Id,
                item.Product.StockQuantity + item.Quantity,
                cancellationToken);

            // Mark item as cancelled
            item.IsCancelled = true;
            cancelledItemsCount++;
        }

        // Update the sale in the repository
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return new CancelSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            IsCancelled = sale.Status == SaleStatus.Cancelled,
            TotalAmount = sale.TotalAmount,
            CancelledItemsCount = cancelledItemsCount,
            CancelledAt = sale.UpdatedAt ?? DateTime.UtcNow,
            Message = $"Sale {sale.SaleNumber} has been successfully cancelled. Stock restored for {cancelledItemsCount} items."
        };
    }
}