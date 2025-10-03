using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Validator for CreateCustomerCommand that defines validation rules for customer creation.
/// </summary>
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateCustomerCommandValidator with defined validation rules.
    /// </summary>
    public CreateCustomerCommandValidator()
    {
        RuleFor(customer => customer.Name)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("Customer name must be between 3 and 100 characters");

        RuleFor(customer => customer.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100)
            .WithMessage("Please provide a valid email address");

        RuleFor(customer => customer.Phone)
            .NotEmpty()
            .Matches(@"^\(\d{2}\) \d{4,5}-\d{4}$")
            .WithMessage("Phone must follow the format (XX) XXXXX-XXXX");

        RuleFor(customer => customer.Address)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Address is required and must not exceed 200 characters");

        RuleFor(customer => customer.BirthDate)
            .LessThan(DateTime.Today)
            .When(customer => customer.BirthDate.HasValue)
            .WithMessage("Birth date must be in the past");
    }
}