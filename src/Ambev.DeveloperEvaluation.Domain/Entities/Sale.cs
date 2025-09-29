using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier for the sale.
    /// </summary>
    public string SaleNumber { get; private set; }
    /// <summary>
    /// Gets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; private set; }
    /// <summary>
    /// Gets the unique identifier for the customer.
    /// </summary>
    public string CustomerId { get; private set; }
    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    public string CustomerName { get; private set; }
    /// <summary>
    /// Gets the unique identifier for the branch.
    /// </summary>
    public string BranchId { get; private set; }
    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    public string BranchName { get; private set; }
    /// <summary>
    /// Gets the total amount for the sale.
    /// </summary>
    public decimal TotalAmount { get; private set; }
    /// <summary>
    /// Gets the list of discounts applied.
    /// </summary>
    public List<decimal> Discounts = new List<decimal>();
    /// <summary>
    /// Gets the date and time of the creation.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets the date and time of the last update to the sale's information.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Private constructor for ORM and factory methods.
    /// </summary>
    private Sale(string saleNumber, DateTime saleDate, string customerId, string customerName, string branchId, string branchName, decimal totalAmount)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Sale()
    {
        CreatedAt = DateTime.UtcNow;   
    }

    /// <summary>
    /// Factory method to create a new Sale instance.
    /// </summary>
    public static Sale Create(string customerId, string customerName, string branchId, string branchName)
    {
        var saleNumber = GenerateSaleNumber();
        var saleDate = DateTime.Now;
        var totalAmount = 0m;

        return new Sale(saleNumber, saleDate, customerId, customerName, branchId, branchName, totalAmount);
    }

    /// <summary>
    /// Simple random sale number generator for demonstration purposes.
    /// </summary>
    private static string GenerateSaleNumber()
    {
        var random = new Random();
        return $"SALE{random.Next(1000, 9999)}";
    }
}