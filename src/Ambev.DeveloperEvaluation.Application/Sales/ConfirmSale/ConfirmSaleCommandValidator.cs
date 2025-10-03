using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

/// <summary>
/// Validator for ConfirmSaleCommand that defines validation rules for confirming a sale.
/// </summary>
public class ConfirmSaleCommandValidator : AbstractValidator<ConfirmSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the ConfirmSaleCommandValidator with defined validation rules.
    /// </summary>
    public ConfirmSaleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}