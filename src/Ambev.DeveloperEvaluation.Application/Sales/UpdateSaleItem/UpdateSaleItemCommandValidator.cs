using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
/// Validator for UpdateSaleItemCommand that enforces business rules and data integrity.
/// </summary>
/// <remarks>
/// This validator ensures that UpdateSaleItem commands meet all business requirements:
/// - Sale ID must be provided and valid
/// - Item ID must be provided and valid  
/// - New quantity must be between 1 and 20 units (per business rules)
/// 
/// The validator works in conjunction with the domain entity validation to ensure
/// comprehensive business rule enforcement including stock validation.
/// </remarks>
public class UpdateSaleItemCommandValidator : AbstractValidator<UpdateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleItemCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules applied:
    /// - SaleId: Required, must not be empty GUID
    /// - ItemId: Required, must not be empty GUID  
    /// - NewQuantity: Required, must be between 1 and 20
    /// </remarks>
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