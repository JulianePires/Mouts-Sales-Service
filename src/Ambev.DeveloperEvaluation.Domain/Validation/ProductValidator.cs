using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Product entity that defines validation rules for product properties.
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    /// <summary>
    /// Initializes a new instance of ProductValidator with validation rules.
    /// </summary>
    public ProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MinimumLength(2)
            .WithMessage("Product name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Product name cannot be longer than 100 characters.");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero.")
            .LessThan(1000000)
            .WithMessage("Product price cannot exceed 1,000,000.");

        RuleFor(product => product.Category)
            .NotEmpty()
            .WithMessage("Product category is required.")
            .MinimumLength(2)
            .WithMessage("Product category must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("Product category cannot be longer than 50 characters.");

        RuleFor(product => product.Description)
            .MaximumLength(500)
            .WithMessage("Product description cannot be longer than 500 characters.")
            .When(product => !string.IsNullOrWhiteSpace(product.Description));

        RuleFor(product => product.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative.");

        RuleFor(product => product.MinStockLevel)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock level cannot be negative.");

        RuleFor(product => product.Image)
            .MaximumLength(300)
            .WithMessage("Product image URL cannot be longer than 300 characters.")
            .When(product => !string.IsNullOrWhiteSpace(product.Image));

        RuleFor(product => product.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Product creation date cannot be in the future.");
    }
}
