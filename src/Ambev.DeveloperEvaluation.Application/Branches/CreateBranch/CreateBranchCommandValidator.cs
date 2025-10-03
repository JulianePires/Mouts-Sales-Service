using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Validator for CreateBranchCommand that defines validation rules for branch creation.
/// </summary>
public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateBranchCommandValidator with defined validation rules.
    /// </summary>
    public CreateBranchCommandValidator()
    {
        RuleFor(branch => branch.Name)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("Branch name must be between 3 and 100 characters");

        RuleFor(branch => branch.Address)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Address is required and must not exceed 200 characters");

        RuleFor(branch => branch.Phone)
            .NotEmpty()
            .Matches(@"^\(\d{2}\) \d{4,5}-\d{4}$")
            .WithMessage("Phone must follow the format (XX) XXXXX-XXXX");

        RuleFor(branch => branch.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100)
            .WithMessage("Please provide a valid email address");

        RuleFor(branch => branch.Manager)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("Manager name must be between 3 and 100 characters");
    }
}