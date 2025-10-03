using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Customer entity class.
/// Tests cover entity creation, validation, business rules, age calculations, and state management.
/// </summary>
public class CustomerTests
{
    /// <summary>
    /// Tests that a valid customer can be created successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid customer data When creating customer Then customer should be created successfully")]
    public void Given_ValidCustomerData_When_CreatingCustomer_Then_CustomerShouldBeCreatedSuccessfully()
    {
        // Arrange
        var name = "John Doe";
        var email = "john.doe@email.com";
        var phone = "+55 11 98765-4321";
        var address = "123 Main Street, City";
        var birthDate = DateTime.Today.AddYears(-25);

        // Act
        var customer = Customer.Create(name, email, phone, address, birthDate);

        // Assert
        customer.Should().NotBeNull();
        customer.Name.Should().Be(name);
        customer.Email.Should().Be(email.ToLowerInvariant());
        customer.Phone.Should().Be(phone);
        customer.Address.Should().Be(address);
        customer.BirthDate.Should().Be(birthDate);
        customer.IsActive.Should().BeTrue();
        customer.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that customer creation fails with empty name.
    /// </summary>
    [Fact(DisplayName = "Given empty name When creating customer Then should throw ArgumentException")]
    public void Given_EmptyName_When_CreatingCustomer_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "";
        var email = "john.doe@email.com";

        // Act & Assert
        var action = () => Customer.Create(name, email, "11122233344");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Customer name cannot be null or empty.*");
    }

    /// <summary>
    /// Tests that customer creation fails with invalid email.
    /// </summary>
    [Fact(DisplayName = "Given invalid email When creating customer Then should throw ArgumentException")]
    public void Given_InvalidEmail_When_CreatingCustomer_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "John Doe";
        var email = "invalid-email";

        // Act & Assert
        var action = () => Customer.Create(name, email, "11122233344");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format.*");
    }

    /// <summary>
    /// Tests that customer age calculation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given customer with birth date When calculating age Then should return correct age")]
    public void Given_CustomerWithBirthDate_When_CalculatingAge_Then_ShouldReturnCorrectAge()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.BirthDate = DateTime.Today.AddYears(-30);

        // Act
        var age = customer.GetAge();

        // Assert
        age.Should().Be(30);
    }

    /// <summary>
    /// Tests that customer without birth date returns null age.
    /// </summary>
    [Fact(DisplayName = "Given customer without birth date When calculating age Then should return null")]
    public void Given_CustomerWithoutBirthDate_When_CalculatingAge_Then_ShouldReturnNull()
    {
        // Arrange
        var customer = CustomerTestData.GenerateCustomerWithoutBirthDate();

        // Act
        var age = customer.GetAge();

        // Assert
        age.Should().BeNull();
    }

    /// <summary>
    /// Tests that legal age verification works correctly for adult customers.
    /// </summary>
    [Fact(DisplayName = "Given customer over 18 When checking legal age Then should return true")]
    public void Given_CustomerOver18_When_CheckingLegalAge_Then_ShouldReturnTrue()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.BirthDate = DateTime.Today.AddYears(-25);

        // Act
        var isOfLegalAge = customer.IsOfLegalAge();

        // Assert
        isOfLegalAge.Should().BeTrue();
    }

    /// <summary>
    /// Tests that legal age verification works correctly for underage customers.
    /// </summary>
    [Fact(DisplayName = "Given customer under 18 When checking legal age Then should return false")]
    public void Given_CustomerUnder18_When_CheckingLegalAge_Then_ShouldReturnFalse()
    {
        // Arrange
        var customer = CustomerTestData.GenerateUnderageCustomer();

        // Act
        var isOfLegalAge = customer.IsOfLegalAge();

        // Assert
        isOfLegalAge.Should().BeFalse();
    }

    /// <summary>
    /// Tests that customer without birth date returns false for legal age.
    /// </summary>
    [Fact(DisplayName = "Given customer without birth date When checking legal age Then should return false")]
    public void Given_CustomerWithoutBirthDate_When_CheckingLegalAge_Then_ShouldReturnFalse()
    {
        // Arrange
        var customer = CustomerTestData.GenerateCustomerWithoutBirthDate();

        // Act
        var isOfLegalAge = customer.IsOfLegalAge();

        // Assert
        isOfLegalAge.Should().BeFalse();
    }

    /// <summary>
    /// Tests that customer can be activated successfully.
    /// </summary>
    [Fact(DisplayName = "Given inactive customer When activating Then customer should be active")]
    public void Given_InactiveCustomer_When_Activating_Then_CustomerShouldBeActive()
    {
        // Arrange
        var customer = CustomerTestData.GenerateInactiveCustomer();
        var originalUpdatedAt = customer.UpdatedAt;

        // Act
        customer.Activate();

        // Assert
        customer.IsActive.Should().BeTrue();
        customer.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    /// <summary>
    /// Tests that customer can be deactivated successfully.
    /// </summary>
    [Fact(DisplayName = "Given active customer When deactivating Then customer should be inactive")]
    public void Given_ActiveCustomer_When_Deactivating_Then_CustomerShouldBeInactive()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var originalUpdatedAt = customer.UpdatedAt;

        // Act
        customer.Deactivate();

        // Assert
        customer.IsActive.Should().BeFalse();
        customer.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    /// <summary>
    /// Tests that customer information can be updated successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid customer When updating info Then customer should be updated")]
    public void Given_ValidCustomer_When_UpdatingInfo_Then_CustomerShouldBeUpdated()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var newName = "Updated Name";
        var newEmail = "updated@email.com";
        var originalUpdatedAt = customer.UpdatedAt;

        // Act
        customer.UpdateInfo(name: newName, email: newEmail);

        // Assert
        customer.Name.Should().Be(newName);
        customer.Email.Should().Be(newEmail.ToLowerInvariant());
        customer.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    /// <summary>
    /// Tests that updating with invalid email throws exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid email When updating customer Then should throw ArgumentException")]
    public void Given_InvalidEmail_When_UpdatingCustomer_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        var invalidEmail = "invalid-email";

        // Act & Assert
        var action = () => customer.UpdateInfo(email: invalidEmail);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format.*");
    }

    /// <summary>
    /// Tests that valid customer passes validation.
    /// </summary>
    [Fact(DisplayName = "Given valid customer When validating Then validation should pass")]
    public void Given_ValidCustomer_When_Validating_Then_ValidationShouldPass()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that customer with invalid name fails validation.
    /// </summary>
    [Fact(DisplayName = "Given customer with invalid name When validating Then validation should fail")]
    public void Given_CustomerWithInvalidName_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var customer = CustomerTestData.GenerateCustomerWithInvalidName();

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("name"));
    }

    /// <summary>
    /// Tests that customer with invalid email fails validation.
    /// </summary>
    [Fact(DisplayName = "Given customer with invalid email When validating Then validation should fail")]
    public void Given_CustomerWithInvalidEmail_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var customer = CustomerTestData.GenerateCustomerWithInvalidEmail();

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("email"));
    }

    /// <summary>
    /// Tests that customer with future birth date fails validation.
    /// </summary>
    [Fact(DisplayName = "Given customer with future birth date When validating Then validation should fail")]
    public void Given_CustomerWithFutureBirthDate_When_Validating_Then_ValidationShouldFail()
    {
        // Arrange
        var customer = CustomerTestData.GenerateCustomerWithFutureBirthDate();

        // Act
        var result = customer.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Birth date"));
    }

    /// <summary>
    /// Tests that customer interface implementation works correctly.
    /// </summary>
    [Fact(DisplayName = "Given customer When accessing via interface Then properties should be exposed correctly")]
    public void Given_Customer_When_AccessingViaInterface_Then_PropertiesShouldBeExposedCorrectly()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();

        // Act
        var iCustomer = (ICustomer)customer;

        // Assert
        iCustomer.Id.Should().Be(customer.Id.ToString());
        iCustomer.Name.Should().Be(customer.Name);
        iCustomer.Email.Should().Be(customer.Email);
        iCustomer.Phone.Should().Be(customer.Phone);
        iCustomer.Address.Should().Be(customer.Address);
        iCustomer.BirthDate.Should().Be(customer.BirthDate);
        iCustomer.IsActive.Should().Be(customer.IsActive);
    }
}
