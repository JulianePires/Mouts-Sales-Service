using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand that defines validation rules for product creation.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateProductCommandValidator with defined validation rules.
    /// </summary>
    public CreateProductCommandValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("Product name must be between 3 and 100 characters");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero");

        RuleFor(product => product.Description)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Product description is required and must not exceed 500 characters");

        RuleFor(product => product.Category)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Product category is required and must not exceed 50 characters");

        RuleFor(product => product.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative");

        RuleFor(product => product.Image)
            .MaximumLength(200)
            .When(product => !string.IsNullOrEmpty(product.Image))
            .WithMessage("Image URL must not exceed 200 characters");
    }
}