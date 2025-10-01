using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for SaleItem entity that defines validation rules for sale item properties.
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    /// <summary>
    /// Initializes a new instance of SaleItemValidator with validation rules.
    /// </summary>
    public SaleItemValidator()
    {
        RuleFor(item => item.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required.");

        RuleFor(item => item.Product)
            .NotNull()
            .WithMessage("Product is required.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 units of the same product in a single sale.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero.")
            .LessThan(1000000)
            .WithMessage("Unit price cannot exceed 1,000,000.");

        RuleFor(item => item.DiscountPercent)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount percentage cannot be negative.")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount percentage cannot exceed 100%.");

        RuleFor(item => item.TotalPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total price cannot be negative.")
            .When(item => !item.IsCancelled);

        RuleFor(item => item.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale item creation date cannot be in the future.");

        // Business rule validation: discount should match quantity
        RuleFor(item => item)
            .Must(ValidateDiscountForQuantity)
            .WithMessage("Discount percentage does not match the quantity rules.")
            .When(item => !item.IsCancelled);
    }

    /// <summary>
    /// Validates that discount percentage matches the business rules for quantity.
    /// 4-9 items = 10%, 10-20 items = 20%, less than 4 = 0%
    /// </summary>
    private static bool ValidateDiscountForQuantity(SaleItem item)
    {
        var expectedDiscount = item.Quantity switch
        {
            >= 10 and <= 20 => 20m,
            >= 4 and < 10 => 10m,
            _ => 0m
        };

        return Math.Abs(item.DiscountPercent - expectedDiscount) < 0.01m;
    }
}
