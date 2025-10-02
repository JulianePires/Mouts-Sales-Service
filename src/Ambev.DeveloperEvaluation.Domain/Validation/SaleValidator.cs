using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Sale entity that defines validation rules for sale properties.
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    /// <summary>
    /// Initializes a new instance of SaleValidator with validation rules.
    /// </summary>
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required.")
            .MinimumLength(5)
            .WithMessage("Sale number must be at least 5 characters long.")
            .MaximumLength(50)
            .WithMessage("Sale number cannot be longer than 50 characters.");

        RuleFor(sale => sale.Customer)
            .NotNull()
            .WithMessage("Customer is required.");

        RuleFor(sale => sale.Branch)
            .NotNull()
            .WithMessage("Branch is required.");

        RuleFor(sale => sale.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("Sale date cannot be more than 1 day in the future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-10))
            .WithMessage("Sale date cannot be more than 10 years in the past.");

        RuleFor(sale => sale.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative.")
            .When(sale => !sale.IsCancelled);

        RuleFor(sale => sale.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow.AddMilliseconds(100))
            .WithMessage("Sale creation date cannot be in the future.");

        // Validate that customer is active
        RuleFor(sale => sale.Customer)
            .Must(customer => customer == null || customer.IsActive)
            .WithMessage("Cannot create sale for inactive customer.")
            .When(sale => sale.Customer != null);

        // Validate that branch is active
        RuleFor(sale => sale.Branch)
            .Must(branch => branch == null || branch.IsActive)
            .WithMessage("Cannot create sale for inactive branch.")
            .When(sale => sale.Branch != null);

        // Business rule: validate maximum 20 units per product across all items
        RuleFor(sale => sale.Items)
            .Must(ValidateMaxUnitsPerProduct)
            .WithMessage("Cannot sell more than 20 units of the same product in a single sale.")
            .When(sale => sale.Items != null && sale.Items.Any());

        // Validate individual sale items
        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator())
            .When(sale => sale.Items != null);
    }

    /// <summary>
    /// Validates that no product exceeds 20 units across all sale items.
    /// </summary>
    private static bool ValidateMaxUnitsPerProduct(List<SaleItem> items)
    {
        if (items == null || !items.Any())
            return true;

        var productQuantities = items
            .Where(i => !i.IsCancelled)
            .GroupBy(i => i.Product?.Id)
            .Select(g => g.Sum(i => i.Quantity))
            .ToList();

        return productQuantities.All(quantity => quantity <= 20);
    }
}
