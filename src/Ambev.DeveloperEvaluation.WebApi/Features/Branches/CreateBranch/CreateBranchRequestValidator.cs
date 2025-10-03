using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

public class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
{
    public CreateBranchRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .MaximumLength(100)
            .WithMessage("Branch name must be less than 100 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Branch address is required")
            .MaximumLength(200)
            .WithMessage("Address must be less than 200 characters");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\([1-9]{2}\)\s[9]?[0-9]{4}-[0-9]{4}$")
            .WithMessage("Phone must be in format (XX) XXXX-XXXX or (XX) 9XXXX-XXXX");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email format is invalid");

        RuleFor(x => x.Manager)
            .NotEmpty()
            .WithMessage("Manager name is required")
            .MaximumLength(100)
            .WithMessage("Manager name must be less than 100 characters");
    }
}