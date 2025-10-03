using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Command for creating a new customer.
/// </summary>
public class CreateCustomerCommand : IRequest<CreateCustomerResult>
{
    /// <summary>
    /// Gets or sets the full name of the customer.
    /// Must contain at least 3 characters.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the customer.
    /// Must be a valid email format and unique in the system.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// Must follow Brazilian phone format.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the physical address of the customer.
    /// Used for delivery purposes.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the birth date of the customer.
    /// Optional field for age verification.
    /// </summary>
    public DateTime? BirthDate { get; set; }


}