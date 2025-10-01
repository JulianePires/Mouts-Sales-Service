using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a customer in the system with personal and contact information.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Customer : BaseEntity, ICustomer
{
    /// <summary>
    /// Gets the full name of the customer.
    /// Must not be null or empty and should contain both first and last names.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the email address of the customer.
    /// Must be a valid email format and is used as a unique identifier for communications.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets the phone number of the customer.
    /// Must be a valid phone number format following the pattern (XX) XXXXX-XXXX.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets the physical address of the customer.
    /// Used for delivery and billing purposes.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets the birth date of the customer.
    /// Used for age verification and marketing purposes.
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Gets the active status of the customer.
    /// Indicates whether the customer can make purchases.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets the date and time when the customer was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the customer information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the unique identifier of the customer.
    /// </summary>
    /// <returns>The customer's ID as a string.</returns>
    string ICustomer.Id => Id.ToString();

    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    /// <returns>The customer name.</returns>
    string ICustomer.Name => Name;

    /// <summary>
    /// Gets the email address of the customer.
    /// </summary>
    /// <returns>The customer email.</returns>
    string ICustomer.Email => Email;

    /// <summary>
    /// Gets the phone number of the customer.
    /// </summary>
    /// <returns>The customer phone number.</returns>
    string ICustomer.Phone => Phone;

    /// <summary>
    /// Gets the address of the customer.
    /// </summary>
    /// <returns>The customer address.</returns>
    string ICustomer.Address => Address;

    /// <summary>
    /// Gets the birth date of the customer.
    /// </summary>
    /// <returns>The customer birth date.</returns>
    DateTime? ICustomer.BirthDate => BirthDate;

    /// <summary>
    /// Gets the active status of the customer.
    /// </summary>
    /// <returns>True if the customer is active; otherwise, false.</returns>
    bool ICustomer.IsActive => IsActive;

    /// <summary>
    /// Initializes a new instance of the Customer class.
    /// </summary>
    public Customer()
    {
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Creates a new customer with the specified information.
    /// </summary>
    /// <param name="name">The customer name.</param>
    /// <param name="email">The customer email.</param>
    /// <param name="phone">The customer phone number.</param>
    /// <param name="address">The customer address.</param>
    /// <param name="birthDate">The customer birth date.</param>
    /// <returns>A new Customer instance.</returns>
    /// <exception cref="ArgumentException">Thrown when required parameters are null or empty.</exception>
    public static Customer Create(string name, string email, string? phone = null, string? address = null, DateTime? birthDate = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email cannot be null or empty.", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format.", nameof(email));

        return new Customer
        {
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Phone = phone?.Trim() ?? string.Empty,
            Address = address?.Trim() ?? string.Empty,
            BirthDate = birthDate
        };
    }

    /// <summary>
    /// Activates the customer account.
    /// Changes the customer's status to Active.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the customer account.
    /// Changes the customer's status to Inactive.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the customer information.
    /// </summary>
    /// <param name="name">The new customer name.</param>
    /// <param name="email">The new customer email.</param>
    /// <param name="phone">The new customer phone number.</param>
    /// <param name="address">The new customer address.</param>
    /// <param name="birthDate">The new customer birth date.</param>
    public void UpdateInfo(string? name = null, string? email = null, string? phone = null, string? address = null, DateTime? birthDate = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();

        if (!string.IsNullOrWhiteSpace(email))
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.", nameof(email));
            Email = email.Trim().ToLowerInvariant();
        }

        if (phone != null)
            Phone = phone.Trim();

        if (address != null)
            Address = address.Trim();

        if (birthDate.HasValue)
            BirthDate = birthDate.Value;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the customer's age based on birth date.
    /// </summary>
    /// <returns>The customer's age in years, or null if birth date is not set.</returns>
    public int? GetAge()
    {
        if (!BirthDate.HasValue)
            return null;

        var today = DateTime.Today;
        var age = today.Year - BirthDate.Value.Year;

        if (BirthDate.Value.Date > today.AddYears(-age))
            age--;

        return age;
    }

    /// <summary>
    /// Checks if the customer is of legal drinking age (18 years or older).
    /// </summary>
    /// <returns>True if the customer is 18 or older; false if younger or birth date is not set.</returns>
    public bool IsOfLegalAge()
    {
        var age = GetAge();
        return age.HasValue && age.Value >= 18;
    }

    /// <summary>
    /// Performs validation of the customer entity using the CustomerValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CustomerValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Validates email format using a simple regex pattern.
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>True if the email format is valid; otherwise, false.</returns>
    private static bool IsValidEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }
}
