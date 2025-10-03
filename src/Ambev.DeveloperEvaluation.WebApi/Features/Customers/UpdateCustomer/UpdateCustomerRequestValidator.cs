using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MinimumLength(3)
            .WithMessage("Name must have at least 3 characters")
            .MaximumLength(100)
            .WithMessage("Name must be less than 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email format is invalid")
            .MaximumLength(200)
            .WithMessage("Email must be less than 200 characters");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\([1-9]{2}\)\s[9]?[0-9]{4}-[0-9]{4}$")
            .WithMessage("Phone must be in Brazilian format (XX) XXXX-XXXX or (XX) 9XXXX-XXXX");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(300)
            .WithMessage("Address must be less than 300 characters");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now)
            .WithMessage("Birth date must be in the past")
            .When(x => x.BirthDate.HasValue);
    }
}