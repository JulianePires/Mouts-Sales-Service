using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Validator for UpdateSaleItemCommand.
/// </summary>
public class UpdateSaleItemCommandValidator : AbstractValidator<UpdateSaleItemCommand>
{
    public UpdateSaleItemCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required.");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required.");

        RuleFor(x => x.NewQuantity)
            .GreaterThan(0)
            .WithMessage("New quantity must be greater than zero.")
            .LessThanOrEqualTo(20)
            .WithMessage("New quantity cannot exceed 20 units per product.");
    }
}
