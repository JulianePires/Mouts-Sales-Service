namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Response model for UpdateCustomer operation.
/// </summary>
public class UpdateCustomerResult
{
    /// <summary>
    /// The unique identifier of the customer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The full name of the customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the customer.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the customer.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The physical address of the customer.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// The birth date of the customer.
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Indicates whether the customer is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The date when the customer was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date when the customer was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}