namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Defines the contract for representing a customer in the system.
/// </summary>
public interface ICustomer
{
    /// <summary>
    /// Gets the unique identifier for the customer.
    /// </summary>
    /// <returns>The Customer ID as string.</returns>
    public string Id { get; }
    
    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    /// <returns>The customer name.</returns>
    public string Name { get; }
    
    /// <summary>
    /// Gets the email address of the customer.
    /// </summary>
    /// <returns>The customer email.</returns>
    public string Email { get; }
    
    /// <summary>
    /// Gets the phone number of the customer.
    /// </summary>
    /// <returns>The customer phone number.</returns>
    public string Phone { get; }
    
    /// <summary>
    /// Gets the address of the customer.
    /// </summary>
    /// <returns>The customer address.</returns>
    public string Address { get; }
    
    /// <summary>
    /// Gets the birth date of the customer.
    /// </summary>
    /// <returns>The customer birth date.</returns>
    public DateTime? BirthDate { get; }
    
    /// <summary>
    /// Gets the active status of the customer.
    /// </summary>
    /// <returns>True if the customer is active; otherwise, false.</returns>
    public bool IsActive { get; }
}