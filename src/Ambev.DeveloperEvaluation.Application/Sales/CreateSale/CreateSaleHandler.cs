using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDiscountService _discountService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="discountService">The discount service for calculating discounts</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IDiscountService discountService,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _discountService = discountService;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="request">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Validate that customer exists
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            throw new ArgumentException($"Customer with ID {request.CustomerId} not found.");

        // Validate that branch exists
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch == null)
            throw new ArgumentException($"Branch with ID {request.BranchId} not found.");

        // Create the sale entity
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = await GenerateSaleNumberAsync(),
            SaleDate = request.SaleDate ?? DateTime.UtcNow,
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            CreatedAt = DateTime.UtcNow,
            IsCancelled = false
        };

        // Process sale items
        var saleItems = new List<SaleItem>();
        decimal totalAmount = 0;

        foreach (var itemRequest in request.Items)
        {
            // Validate that product exists and is available
            var product = await _productRepository.GetByIdAsync(itemRequest.ProductId, cancellationToken);
            if (product == null)
                throw new ArgumentException($"Product with ID {itemRequest.ProductId} not found.");

            // Check if product is available in requested quantity
            if (!await _productRepository.IsAvailableAsync(itemRequest.ProductId, itemRequest.Quantity, cancellationToken))
                throw new InvalidOperationException($"Product {product.Name} is not available in the requested quantity ({itemRequest.Quantity}).");

            // Calculate discount based on quantity
            var unitPrice = product.Price;
            var discountPercent = _discountService.CalculateDiscount(itemRequest.Quantity, unitPrice);
            var totalPrice = (unitPrice * itemRequest.Quantity) * (1 - (discountPercent.Amount / 100));

            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                SaleId = sale.Id,
                Product = product,
                Quantity = itemRequest.Quantity,
                UnitPrice = unitPrice,
                DiscountPercent = discountPercent.Amount,
                TotalPrice = totalPrice,
                IsCancelled = false
            };

            saleItems.Add(saleItem);
            totalAmount += totalPrice;

            // Reduce product stock
            await _productRepository.ReduceStockAsync(itemRequest.ProductId, itemRequest.Quantity, cancellationToken);
        }

        sale.Items = saleItems;
        sale.TotalAmount = totalAmount;

        // Save the sale
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }

    /// <summary>
    /// Generates a unique sale number
    /// </summary>
    /// <returns>A unique sale number</returns>
    private static Task<string> GenerateSaleNumberAsync()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return Task.FromResult($"SAL{timestamp}{random}");
    }
}