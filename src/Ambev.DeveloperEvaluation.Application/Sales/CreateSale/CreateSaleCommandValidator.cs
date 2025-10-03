using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - CustomerId: Must not be empty and must be a valid GUID
    /// - BranchId: Must not be empty and must be a valid GUID
    /// - Items: Must contain at least one item and not exceed business limits
    /// - Each item must have valid ProductId and Quantity within business rules
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required.");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Sale must contain at least one item.")
            .Must(items => items == null || items.Count <= 20)
            .WithMessage("Sale cannot contain more than 20 different items (business rule).");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty()
                    .WithMessage("Product ID is required for each item.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than 0.")
                    .LessThanOrEqualTo(20)
                    .WithMessage("Quantity cannot exceed 20 units per product (business rule).");
            });

        RuleFor(x => x.SaleDate)
            .Must(date => !date.HasValue || date.Value <= DateTime.UtcNow.AddDays(1))
            .WithMessage("Sale date cannot be in the future beyond tomorrow.");

        // Validate that there are no duplicate products in the sale
        RuleFor(x => x.Items)
            .Must(items => items == null || items.GroupBy(i => i.ProductId).All(g => g.Count() == 1))
            .WithMessage("Cannot have duplicate products in the same sale. Adjust quantities instead.");
    }
}