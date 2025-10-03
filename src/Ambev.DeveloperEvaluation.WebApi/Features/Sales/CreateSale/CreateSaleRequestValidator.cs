using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest that defines validation rules for creating a sale.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules.
    /// </summary>
    public CreateSaleRequestValidator()
    {
        RuleFor(request => request.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(request => request.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        RuleFor(request => request.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("Sale date cannot be more than 1 day in the future")
            .When(request => request.SaleDate.HasValue);

        RuleFor(request => request.Items)
            .NotEmpty()
            .WithMessage("At least one item is required")
            .Must(items => items.Count <= 20)
            .WithMessage("Maximum of 20 different products per sale allowed");

        RuleForEach(request => request.Items)
            .SetValidator(new CreateSaleItemRequestValidator());
    }
}

/// <summary>
/// Validator for CreateSaleItemRequest that defines validation rules for sale items.
/// </summary>
public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleItemRequestValidator with defined validation rules.
    /// </summary>
    public CreateSaleItemRequestValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThan(20)
            .WithMessage("Maximum quantity per item is 20 units");
    }
}