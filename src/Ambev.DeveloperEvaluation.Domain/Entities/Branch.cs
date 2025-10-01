using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a branch in the system with location and management information.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Branch : BaseEntity, IBranch
{
    /// <summary>
    /// Gets the name of the branch.
    /// Must not be null or empty and should be unique within the system.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the physical address of the branch.
    /// Must be a valid address format.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets the phone number of the branch.
    /// Must be a valid phone number format following the pattern (XX) XXXXX-XXXX.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets the email address of the branch.
    /// Must be a valid email format for business communications.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets the name of the branch manager.
    /// Must not be null or empty.
    /// </summary>
    public string Manager { get; set; } = string.Empty;

    /// <summary>
    /// Gets the active status of the branch.
    /// Indicates whether the branch is operational and accepting sales.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets the date and time when the branch was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the branch information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the unique identifier of the branch.
    /// </summary>
    /// <returns>The branch's ID as a string.</returns>
    string IBranch.Id => Id.ToString();

    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    /// <returns>The branch name.</returns>
    string IBranch.Name => Name;

    /// <summary>
    /// Gets the address of the branch.
    /// </summary>
    /// <returns>The branch address.</returns>
    string IBranch.Address => Address;

    /// <summary>
    /// Gets the phone number of the branch.
    /// </summary>
    /// <returns>The branch phone number.</returns>
    string IBranch.Phone => Phone;

    /// <summary>
    /// Gets the email of the branch.
    /// </summary>
    /// <returns>The branch email.</returns>
    string IBranch.Email => Email;

    /// <summary>
    /// Gets the branch manager name.
    /// </summary>
    /// <returns>The branch manager name.</returns>
    string IBranch.Manager => Manager;

    /// <summary>
    /// Gets the active status of the branch.
    /// </summary>
    /// <returns>True if the branch is active; otherwise, false.</returns>
    bool IBranch.IsActive => IsActive;

    /// <summary>
    /// Initializes a new instance of the Branch class.
    /// </summary>
    public Branch()
    {
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Creates a new branch with the specified information.
    /// </summary>
    /// <param name="name">The branch name.</param>
    /// <param name="address">The branch address.</param>
    /// <param name="phone">The branch phone number.</param>
    /// <param name="email">The branch email.</param>
    /// <param name="manager">The branch manager name.</param>
    /// <returns>A new Branch instance.</returns>
    /// <exception cref="ArgumentException">Thrown when required parameters are null or empty.</exception>
    public static Branch Create(string name, string address, string phone, string email, string manager)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Branch name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Branch address cannot be null or empty.", nameof(address));

        if (string.IsNullOrWhiteSpace(manager))
            throw new ArgumentException("Branch manager cannot be null or empty.", nameof(manager));

        return new Branch
        {
            Name = name.Trim(),
            Address = address.Trim(),
            Phone = phone?.Trim() ?? string.Empty,
            Email = email?.Trim() ?? string.Empty,
            Manager = manager.Trim()
        };
    }

    /// <summary>
    /// Activates the branch.
    /// Changes the branch's status to Active.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the branch.
    /// Changes the branch's status to Inactive.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the branch information.
    /// </summary>
    /// <param name="name">The new branch name.</param>
    /// <param name="address">The new branch address.</param>
    /// <param name="phone">The new branch phone number.</param>
    /// <param name="email">The new branch email.</param>
    /// <param name="manager">The new branch manager name.</param>
    public void UpdateInfo(string? name = null, string? address = null, string? phone = null, string? email = null, string? manager = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();

        if (!string.IsNullOrWhiteSpace(address))
            Address = address.Trim();

        if (phone != null)
            Phone = phone.Trim();

        if (email != null)
            Email = email.Trim();

        if (!string.IsNullOrWhiteSpace(manager))
            Manager = manager.Trim();

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Performs validation of the branch entity using the BranchValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new BranchValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
