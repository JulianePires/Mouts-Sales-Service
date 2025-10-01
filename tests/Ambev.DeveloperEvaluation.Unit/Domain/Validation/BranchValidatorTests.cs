using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the BranchValidator class.
/// Tests cover validation of all branch properties including name, address,
/// manager, email, phone, and creation date requirements.
/// </summary>
public class BranchValidatorTests
{
    private readonly BranchValidator _validator;

    public BranchValidatorTests()
    {
        _validator = new BranchValidator();
    }

    /// <summary>
    /// Tests that validation passes when all branch properties are valid.
    /// </summary>
    [Fact(DisplayName = "Valid branch should pass all validation rules")]
    public void Given_ValidBranch_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when branch name is empty.
    /// </summary>
    [Fact(DisplayName = "Empty branch name should fail validation")]
    public void Given_EmptyBranchName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Name = string.Empty;

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Name)
            .WithErrorMessage("Branch name is required.");
    }

    /// <summary>
    /// Tests that validation fails when branch name is too short.
    /// </summary>
    [Fact(DisplayName = "Branch name too short should fail validation")]
    public void Given_BranchNameTooShort_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Name = "A";

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Name)
            .WithErrorMessage("Branch name must be at least 2 characters long.");
    }

    /// <summary>
    /// Tests that validation fails when branch name is too long.
    /// </summary>
    [Fact(DisplayName = "Branch name too long should fail validation")]
    public void Given_BranchNameTooLong_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Name = new string('A', 101);

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Name)
            .WithErrorMessage("Branch name cannot be longer than 100 characters.");
    }

    /// <summary>
    /// Tests that validation fails when branch address is empty.
    /// </summary>
    [Fact(DisplayName = "Empty branch address should fail validation")]
    public void Given_EmptyBranchAddress_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Address = string.Empty;

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Address)
            .WithErrorMessage("Branch address is required.");
    }

    /// <summary>
    /// Tests that validation fails when branch address is too short.
    /// </summary>
    [Fact(DisplayName = "Branch address too short should fail validation")]
    public void Given_BranchAddressTooShort_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Address = "123";

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Address)
            .WithErrorMessage("Branch address must be at least 10 characters long.");
    }

    /// <summary>
    /// Tests that validation fails when branch manager is empty.
    /// </summary>
    [Fact(DisplayName = "Empty branch manager should fail validation")]
    public void Given_EmptyBranchManager_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Manager = string.Empty;

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Manager)
            .WithErrorMessage("Branch manager name is required.");
    }

    /// <summary>
    /// Tests that validation fails when branch has invalid email format.
    /// </summary>
    [Fact(DisplayName = "Invalid email format should fail validation")]
    public void Given_InvalidEmailFormat_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Email = "invalid-email";

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Email);
    }

    /// <summary>
    /// Tests that validation passes when email is empty (optional field).
    /// </summary>
    [Fact(DisplayName = "Empty email should pass validation")]
    public void Given_EmptyEmail_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Email = string.Empty;

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldNotHaveValidationErrorFor(b => b.Email);
    }

    /// <summary>
    /// Tests that validation fails when creation date is in the future.
    /// </summary>
    [Fact(DisplayName = "Future creation date should fail validation")]
    public void Given_FutureCreationDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.CreatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        var result = _validator.TestValidate(branch);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.CreatedAt)
            .WithErrorMessage("Branch creation date cannot be in the future.");
    }
}
