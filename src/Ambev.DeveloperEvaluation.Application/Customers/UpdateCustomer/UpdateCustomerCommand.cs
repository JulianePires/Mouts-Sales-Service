using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Command for updating an existing customer.
/// </summary>
public class UpdateCustomerCommand : IRequest<UpdateCustomerResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the customer to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the customer.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the email address of the customer.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the physical address of the customer.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the birth date of the customer.
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Gets or sets whether the customer should be activated or deactivated.
    /// </summary>
    public bool? IsActive { get; set; }
}