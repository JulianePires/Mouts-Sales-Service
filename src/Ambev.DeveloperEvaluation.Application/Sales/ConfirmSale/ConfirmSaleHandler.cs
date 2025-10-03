using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

/// <summary>
/// Handler for processing ConfirmSaleCommand requests
/// </summary>
public class ConfirmSaleHandler : IRequestHandler<ConfirmSaleCommand, ConfirmSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of ConfirmSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public ConfirmSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the ConfirmSaleCommand request
    /// </summary>
    /// <param name="request">The ConfirmSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The confirmation result</returns>
    public async Task<ConfirmSaleResult> Handle(ConfirmSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new ConfirmSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found.");

        if (sale.Status == SaleStatus.Confirmed)
            throw new InvalidOperationException($"Sale {sale.SaleNumber} is already confirmed.");

        if (sale.Status == SaleStatus.Cancelled)
            throw new InvalidOperationException($"Sale {sale.SaleNumber} is cancelled and cannot be confirmed.");

        // Use domain method to confirm the sale
        sale.Confirm();

        // Update the sale in the repository
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return new ConfirmSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            TotalAmount = sale.TotalAmount,
            ConfirmedAt = sale.UpdatedAt ?? DateTime.UtcNow,
            Message = $"Sale {sale.SaleNumber} has been successfully confirmed."
        };
    }
}