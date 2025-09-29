namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Defines the contract for representing a sale in the system.
/// </summary>
public interface ISale
{
    /// <summary>
    /// Gets the unique identifier for the sale.
    /// </summary>
    /// <returns>The Sale ID as string.</returns>
    public string Id { get; }
    
    /// <summary>
    /// Gets the sale number.
    /// </summary>
    /// <returns>The sale number.</returns>
    public string SaleNumber { get; }
    
    /// <summary>
    /// Gets the date when the sale was made.
    /// </summary>
    /// <returns>The sale date.</returns>
    public DateTime SaleDate { get; }
    
    /// <summary>
    /// Gets the data of the customer.
    /// </summary>
    /// <returns>The customer data.</returns>
    public ICustomer Customer { get; }
    
    /// <summary>
    /// Gets the data for the branch where the sale was made.
    /// </summary>
    /// <returns>The branch data.</returns>
    public IBranch Branch { get; }
    
    /// <summary>
    /// Gets the list of items included in the sale.
    /// </summary>
    /// <returns>A list of sale items.</returns>
    public IList<ISaleItem> Items { get; }
    
    /// <summary>
    /// Gets the total amount for the sale.
    /// </summary>
    /// <returns>The total amount as decimal.</returns>
    public decimal TotalAmount { get; }
}