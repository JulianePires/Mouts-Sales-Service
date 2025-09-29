namespace Ambev.DeveloperEvaluation.Common.Security;

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
}