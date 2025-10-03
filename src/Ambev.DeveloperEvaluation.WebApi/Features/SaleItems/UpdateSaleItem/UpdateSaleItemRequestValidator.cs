using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.UpdateSaleItem;

/// <summary>
/// Validator for UpdateSaleItemRequest
/// </summary>
public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateSaleItemRequest
    /// </summary>
    public UpdateSaleItemRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot have more than 20 identical items");
    }
}