using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Customer entity that defines validation rules for customer properties.
/// </summary>
public class CustomerValidator : AbstractValidator<Customer>
{
    /// <summary>
    /// Initializes a new instance of CustomerValidator with validation rules.
    /// </summary>
    public CustomerValidator()
    {
        RuleFor(customer => customer.Name)
            .NotEmpty()
            .WithMessage("Customer name is required.")
            .MinimumLength(3)
            .WithMessage("Customer name must be at least 3 characters long.")
            .MaximumLength(100)
            .WithMessage("Customer name cannot be longer than 100 characters.");

        RuleFor(user => user.Email).SetValidator(new EmailValidator());

        RuleFor(user => user.Phone)
            .Matches(@"^\+[1-9]\d{10,14}$")
            .WithMessage("Phone number must start with '+' followed by 11-15 digits.");

        RuleFor(customer => customer.BirthDate)
            .LessThan(DateTime.Today)
            .WithMessage("Birth date cannot be today or in the future.")
            .GreaterThan(DateTime.Today.AddYears(-120))
            .WithMessage("Birth date cannot be more than 120 years ago.")
            .When(customer => customer.BirthDate.HasValue);

        RuleFor(customer => customer.Address)
            .MaximumLength(200)
            .WithMessage("Customer address cannot be longer than 200 characters.")
            .When(customer => !string.IsNullOrWhiteSpace(customer.Address));

        RuleFor(customer => customer.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Customer creation date cannot be in the future.");
    }
}
