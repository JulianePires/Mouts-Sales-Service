using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ConfirmSale;

/// <summary>
/// Validator for ConfirmSaleRequest
/// </summary>
public class ConfirmSaleRequestValidator : AbstractValidator<ConfirmSaleRequest>
{
    /// <summary>
    /// Initializes validation rules for ConfirmSaleRequest
    /// </summary>
    public ConfirmSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}