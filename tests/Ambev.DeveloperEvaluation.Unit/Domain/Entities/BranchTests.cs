using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Branch entity class.
/// Tests cover entity creation, validation, business rules, and state management.
/// </summary>
public class BranchTests
{
    /// <summary>
    /// Tests that a valid branch can be created successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid branch data When creating branch Then branch should be created successfully")]
    public void Given_ValidBranchData_When_CreatingBranch_Then_BranchShouldBeCreatedSuccessfully()
    {
        // Arrange
        var name = "Downtown Branch";
        var address = "123 Main Street, Downtown City";
        var phone = "+55 11 98765-4321";
        var email = "downtown@company.com";
        var manager = "John Manager";

        // Act
        var branch = Branch.Create(name, address, phone, email, manager);

        // Assert
        branch.Should().NotBeNull();
        branch.Name.Should().Be(name);
        branch.Address.Should().Be(address);
        branch.Phone.Should().Be(phone);
        branch.Email.Should().Be(email);
        branch.Manager.Should().Be(manager);
        branch.IsActive.Should().BeTrue();
        branch.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that branch creation fails with empty name.
    /// </summary>
    [Fact(DisplayName = "Given empty name When creating branch Then should throw ArgumentException")]
    public void Given_EmptyName_When_CreatingBranch_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "";
        var address = "123 Main Street, Downtown City";
        var phone = "+55 11 98765-4321";
        var email = "downtown@company.com";
        var manager = "John Manager";

        // Act & Assert
        var action = () => Branch.Create(name, address, phone, email, manager);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Branch name cannot be null or empty.*");
    }

    /// <summary>
    /// Tests that branch creation fails with empty address.
    /// </summary>
    [Fact(DisplayName = "Given empty address When creating branch Then should throw ArgumentException")]
    public void Given_EmptyAddress_When_CreatingBranch_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "Downtown Branch";
        var address = "";
        var phone = "+55 11 98765-4321";
        var email = "downtown@company.com";
        var manager = "John Manager";

        // Act & Assert
        var action = () => Branch.Create(name, address, phone, email, manager);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Branch address cannot be null or empty.*");
    }

    /// <summary>
    /// Tests that branch creation fails with empty manager.
    /// </summary>
    [Fact(DisplayName = "Given empty manager When creating branch Then should throw ArgumentException")]
    public void Given_EmptyManager_When_CreatingBranch_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "Downtown Branch";
        var address = "123 Main Street, Downtown City";
        var phone = "+55 11 98765-4321";
        var email = "downtown@company.com";
        var manager = "";

        // Act & Assert
        var action = () => Branch.Create(name, address, phone, email, manager);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Branch manager cannot be null or empty.*");
    }

    /// <summary>
    /// Tests that branch can be activated successfully.
    /// </summary>
    [Fact(DisplayName = "Given inactive branch When activating Then branch should be active")]
    public void Given_InactiveBranch_When_Activating_Then_BranchShouldBeActive()
    {
        // Arrange
        var branch = BranchTestData.GenerateInactiveBranch();
        var originalUpdatedAt = branch.UpdatedAt;

        // Act
        branch.Activate();

        // Assert
        branch.IsActive.Should().BeTrue();
        branch.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    /// <summary>
    /// Tests that branch can be deactivated successfully.
    /// </summary>
    [Fact(DisplayName = "Given active branch When deactivating Then branch should be inactive")]
    public void Given_ActiveBranch_When_Deactivating_Then_BranchShouldBeInactive()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        var originalUpdatedAt = branch.UpdatedAt;

        // Act
        branch.Deactivate();

        // Assert
        branch.IsActive.Should().BeFalse();
        branch.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    /// <summary>
    /// Tests that branch information can be updated successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid branch When updating info Then branch should be updated")]
    public void Given_ValidBranch_When_UpdatingInfo_Then_BranchShouldBeUpdated()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        var newName = "Updated Branch Name";
        var newManager = "Updated Manager";
        var originalUpdatedAt = branch.UpdatedAt;

        // Act
        branch.UpdateInfo(name: newName, manager: newManager);

        // Assert
        branch.Name.Should().Be(newName);
        branch.Manager.Should().Be(newManager);
        branch.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    /// <summary>
    /// Tests that valid branch passes validation.
    /// </summary>
    [Fact(DisplayName = "Given valid branch When validating Then validation should pass")]
    public void Given_ValidBranch_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act
        var result = branch.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that branch with invalid name fails validation.
    /// </summary>
    [Fact(DisplayName = "Given branch with invalid name When validating Then validation should fail")]
    public void Given_BranchWithInvalidName_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var branch = BranchTestData.GenerateBranchWithInvalidName();

        // Act
        var result = branch.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Error == nameof(Branch.Name));
    }

    /// <summary>
    /// Tests that branch with invalid address fails validation.
    /// </summary>
    [Fact(DisplayName = "Given branch with invalid address When validating Then validation should fail")]
    public void Given_BranchWithInvalidAddress_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var branch = BranchTestData.GenerateBranchWithInvalidAddress();

        // Act
        var result = branch.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Error == nameof(Branch.Address));
    }

    /// <summary>
    /// Tests that branch with invalid email fails validation.
    /// </summary>
    [Fact(DisplayName = "Given branch with invalid email When validating Then validation should fail")]
    public void Given_BranchWithInvalidEmail_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var branch = BranchTestData.GenerateBranchWithInvalidEmail();

        // Act
        var result = branch.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Error == nameof(Branch.Email));
    }

    /// <summary>
    /// Tests that branch interface implementation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given branch When accessing via interface Then properties should be exposed correctly")]
    public void Given_Branch_When_AccessingViaInterface_Then_PropertiesShouldBeExposedCorrectly()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act
        var iBranch = (IBranch)branch;

        // Assert
        iBranch.Id.Should().Be(branch.Id.ToString());
        iBranch.Name.Should().Be(branch.Name);
        iBranch.Address.Should().Be(branch.Address);
        iBranch.Phone.Should().Be(branch.Phone);
        iBranch.Email.Should().Be(branch.Email);
        iBranch.Manager.Should().Be(branch.Manager);
        iBranch.IsActive.Should().Be(branch.IsActive);
    }
}
