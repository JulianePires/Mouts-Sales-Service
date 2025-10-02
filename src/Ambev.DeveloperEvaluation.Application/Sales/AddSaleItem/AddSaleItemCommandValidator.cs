using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

/// <summary>
/// Validator for AddSaleItemCommand that enforces business rules and data integrity.
/// </summary>
/// <remarks>
/// This validator ensures that AddSaleItem commands meet all business requirements:
/// - Sale ID must be provided and valid
/// - Product ID must be provided and valid  
/// - Quantity must be between 1 and 20 units (per business rules)
/// - Unit price (if provided) must be positive
/// 
/// The validator works in conjunction with the domain entity validation to ensure
/// comprehensive business rule enforcement.
/// </remarks>
public class AddSaleItemCommandValidator : AbstractValidator<AddSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the AddSaleItemCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules applied:
    /// - SaleId: Required, must not be empty GUID
    /// - ProductId: Required, must not be empty GUID  
    /// - Quantity: Required, must be between 1 and 20
    /// - UnitPrice: Optional, but if provided must be positive
    /// </remarks>
    public AddSaleItemCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20)
            .WithMessage("Quantity cannot exceed 20 units per product.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .When(x => x.UnitPrice.HasValue)
            .WithMessage("Unit price must be greater than zero when provided.");
    }
}