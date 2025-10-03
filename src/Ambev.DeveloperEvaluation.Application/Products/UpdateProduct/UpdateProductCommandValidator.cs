using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Validator for UpdateProductCommand that defines validation rules for product updates.
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateProductCommandValidator with defined validation rules.
    /// </summary>
    public UpdateProductCommandValidator()
    {
        RuleFor(product => product.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(product => product.Name)
            .Length(3, 100)
            .When(product => !string.IsNullOrEmpty(product.Name))
            .WithMessage("Product name must be between 3 and 100 characters");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .When(product => product.Price.HasValue)
            .WithMessage("Product price must be greater than zero");

        RuleFor(product => product.Description)
            .MaximumLength(500)
            .When(product => !string.IsNullOrEmpty(product.Description))
            .WithMessage("Product description must not exceed 500 characters");

        RuleFor(product => product.Category)
            .MaximumLength(50)
            .When(product => !string.IsNullOrEmpty(product.Category))
            .WithMessage("Product category must not exceed 50 characters");

        RuleFor(product => product.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .When(product => product.StockQuantity.HasValue)
            .WithMessage("Stock quantity cannot be negative");

        RuleFor(product => product.Image)
            .MaximumLength(200)
            .When(product => !string.IsNullOrEmpty(product.Image))
            .WithMessage("Image URL must not exceed 200 characters");
    }
}