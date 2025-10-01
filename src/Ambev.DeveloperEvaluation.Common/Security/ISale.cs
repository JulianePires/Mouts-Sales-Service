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
    /// Gets the customer data.
    /// </summary>
    /// <returns>The customer information.</returns>
    public ICustomer Customer { get; }
    
    /// <summary>
    /// Gets the data of the branch where the sale was made.
    /// </summary>
    /// <returns>The branch information.</returns>
    public IBranch Branch { get; }
    
    /// <summary>
    /// Gets the list of items included in the sale.
    /// </summary>
    /// <returns>A readonly collection of sale items.</returns>
    public IReadOnlyCollection<ISaleItem> Items { get; }
    
    /// <summary>
    /// Gets the total amount for the sale.
    /// </summary>
    /// <returns>The total sale amount.</returns>
    public decimal TotalAmount { get; }
    
    /// <summary>
    /// Gets the cancellation status of the sale.
    /// </summary>
    /// <returns>True if the sale is cancelled; otherwise, false.</returns>
    public bool IsCancelled { get; }
    
    /// <summary>
    /// Gets the date when the sale was created.
    /// </summary>
    /// <returns>The sale creation date.</returns>
    public DateTime CreatedAt { get; }
    
    /// <summary>
    /// Gets the date of the last update to the sale.
    /// </summary>
    /// <returns>The last update date, if any.</returns>
    public DateTime? UpdatedAt { get; }
}