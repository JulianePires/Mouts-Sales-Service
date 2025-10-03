using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.AddSaleItem;

/// <summary>
/// Validator for AddSaleItemRequest
/// </summary>
public class AddSaleItemRequestValidator : AbstractValidator<AddSaleItemRequest>
{
    /// <summary>
    /// Initializes validation rules for AddSaleItemRequest
    /// </summary>
    public AddSaleItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThan(20)
            .WithMessage("Maximum quantity per item is 20 units");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero")
            .When(x => x.UnitPrice.HasValue);
    }
}