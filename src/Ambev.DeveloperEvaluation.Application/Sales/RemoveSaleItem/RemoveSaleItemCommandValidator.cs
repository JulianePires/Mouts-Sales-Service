using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Validator for RemoveSaleItemCommand that enforces business rules and data integrity.
/// </summary>
/// <remarks>
/// This validator ensures that RemoveSaleItem commands meet all business requirements:
/// - Sale ID must be provided and valid
/// - Item ID must be provided and valid  
/// 
/// The validator works in conjunction with the domain entity validation to ensure
/// comprehensive business rule enforcement including existence checks.
/// </remarks>
public class RemoveSaleItemCommandValidator : AbstractValidator<RemoveSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the RemoveSaleItemCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules applied:
    /// - SaleId: Required, must not be empty GUID
    /// - ItemId: Required, must not be empty GUID  
    /// </remarks>
    public RemoveSaleItemCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required.");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required.");
    }
}