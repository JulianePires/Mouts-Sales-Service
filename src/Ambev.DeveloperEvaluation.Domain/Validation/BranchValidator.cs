using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Branch entity that defines validation rules for branch properties.
/// </summary>
public class BranchValidator : AbstractValidator<Branch>
{
    /// <summary>
    /// Initializes a new instance of BranchValidator with validation rules.
    /// </summary>
    public BranchValidator()
    {
        RuleFor(branch => branch.Name)
            .NotEmpty()
            .WithMessage("Branch name is required.")
            .MinimumLength(2)
            .WithMessage("Branch name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Branch name cannot be longer than 100 characters.");

        RuleFor(branch => branch.Address)
            .NotEmpty()
            .WithMessage("Branch address is required.")
            .MinimumLength(10)
            .WithMessage("Branch address must be at least 10 characters long.")
            .MaximumLength(200)
            .WithMessage("Branch address cannot be longer than 200 characters.");

        RuleFor(branch => branch.Manager)
            .NotEmpty()
            .WithMessage("Branch manager name is required.")
            .MinimumLength(2)
            .WithMessage("Branch manager name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Branch manager name cannot be longer than 100 characters.");

        RuleFor(user => user.Email).SetValidator(new EmailValidator());

        RuleFor(user => user.Phone)
            .Matches(@"^\+[1-9]\d{10,14}$")
            .WithMessage("Phone number must start with '+' followed by 11-15 digits.");

        RuleFor(branch => branch.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Branch creation date cannot be in the future.");
    }
}
