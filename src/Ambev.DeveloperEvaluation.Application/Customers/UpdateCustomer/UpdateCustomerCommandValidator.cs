using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Validator for UpdateCustomerCommand that defines validation rules for customer updates.
/// </summary>
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateCustomerCommandValidator with defined validation rules.
    /// </summary>
    public UpdateCustomerCommandValidator()
    {
        RuleFor(customer => customer.Id)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(customer => customer.Name)
            .Length(3, 100)
            .When(customer => !string.IsNullOrEmpty(customer.Name))
            .WithMessage("Customer name must be between 3 and 100 characters");

        RuleFor(customer => customer.Email)
            .EmailAddress()
            .MaximumLength(100)
            .When(customer => !string.IsNullOrEmpty(customer.Email))
            .WithMessage("Please provide a valid email address");

        RuleFor(customer => customer.Phone)
            .Matches(@"^\(\d{2}\) \d{4,5}-\d{4}$")
            .When(customer => !string.IsNullOrEmpty(customer.Phone))
            .WithMessage("Phone must follow the format (XX) XXXXX-XXXX");

        RuleFor(customer => customer.Address)
            .MaximumLength(200)
            .When(customer => !string.IsNullOrEmpty(customer.Address))
            .WithMessage("Address must not exceed 200 characters");

        RuleFor(customer => customer.BirthDate)
            .LessThan(DateTime.Today)
            .When(customer => customer.BirthDate.HasValue)
            .WithMessage("Birth date must be in the past");
    }
}