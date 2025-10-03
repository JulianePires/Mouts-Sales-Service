using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the CustomerValidator class.
/// Tests cover validation of all customer properties including name, email,
/// phone, birth date, address, and creation date requirements.
/// </summary>
public class CustomerValidatorTests
{
    private readonly CustomerValidator _validator;

    public CustomerValidatorTests()
    {
        _validator = new CustomerValidator();
    }

    /// <summary>
    /// Tests that validation passes when all customer properties are valid.
    /// </summary>
    [Fact(DisplayName = "Valid customer should pass all validation rules")]
    public void Given_ValidCustomer_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when customer name is empty.
    /// </summary>
    [Fact(DisplayName = "Empty customer name should fail validation")]
    public void Given_EmptyCustomerName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Name = string.Empty;

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Customer name is required.");
    }

    /// <summary>
    /// Tests that validation fails when customer name is too short.
    /// </summary>
    [Fact(DisplayName = "Customer name too short should fail validation")]
    public void Given_CustomerNameTooShort_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Name = "A";

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Customer name must be at least 3 characters long.");
    }

    /// <summary>
    /// Tests that validation fails when customer name is too long.
    /// </summary>
    [Fact(DisplayName = "Customer name too long should fail validation")]
    public void Given_CustomerNameTooLong_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Name = new string('A', 101);

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Customer name cannot be longer than 100 characters.");
    }

    /// <summary>
    /// Tests that validation fails when customer email is empty.
    /// </summary>
    [Fact(DisplayName = "Empty customer email should fail validation")]
    public void Given_EmptyCustomerEmail_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Email = string.Empty;

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    /// <summary>
    /// Tests that validation fails when customer email has invalid format.
    /// </summary>
    [Fact(DisplayName = "Invalid email format should fail validation")]
    public void Given_InvalidEmailFormat_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Email = "invalid-email";

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    /// <summary>
    /// Tests that validation fails when birth date is in the future.
    /// </summary>
    [Fact(DisplayName = "Future birth date should fail validation")]
    public void Given_FutureBirthDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.BirthDate = DateTime.Today.AddDays(1);

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.BirthDate)
            .WithErrorMessage("Birth date cannot be today or in the future.");
    }

    /// <summary>
    /// Tests that validation fails when birth date is too far in the past.
    /// </summary>
    [Fact(DisplayName = "Birth date too old should fail validation")]
    public void Given_BirthDateTooOld_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.BirthDate = DateTime.Today.AddYears(-121);

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.BirthDate)
            .WithErrorMessage("Birth date cannot be more than 120 years ago.");
    }

    /// <summary>
    /// Tests that validation passes when birth date is null (optional field).
    /// </summary>
    [Fact(DisplayName = "Null birth date should pass validation")]
    public void Given_NullBirthDate_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.BirthDate = null;

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.BirthDate);
    }

    /// <summary>
    /// Tests that validation fails when address is too long.
    /// </summary>
    [Fact(DisplayName = "Address too long should fail validation")]
    public void Given_AddressTooLong_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Address = new string('A', 201);

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Address)
            .WithErrorMessage("Customer address cannot be longer than 200 characters.");
    }

    /// <summary>
    /// Tests that validation passes when address is empty (optional field).
    /// </summary>
    [Fact(DisplayName = "Empty address should pass validation")]
    public void Given_EmptyAddress_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Address = string.Empty;

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Address);
    }

    /// <summary>
    /// Tests that validation fails when creation date is in the future.
    /// </summary>
    [Fact(DisplayName = "Future creation date should fail validation")]
    public void Given_FutureCreationDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.CreatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        var result = _validator.TestValidate(customer);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.CreatedAt)
            .WithErrorMessage("Customer creation date cannot be in the future.");
    }
}
