using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a sale is modified.
/// </summary>
public class SaleModified : DomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the modified sale.
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Gets the sale number for identification.
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Gets the customer ID associated with the sale.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the branch ID where the sale belongs.
    /// </summary>
    public Guid BranchId { get; }

    /// <summary>
    /// Gets the new total amount of the sale after modification.
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// Gets the current number of active items in the sale.
    /// </summary>
    public int ItemCount { get; }

    /// <summary>
    /// Gets the type of modification made to the sale.
    /// </summary>
    public string ModificationType { get; }

    /// <summary>
    /// Gets additional details about the modification.
    /// </summary>
    public string ModificationDetails { get; }

    /// <summary>
    /// Initializes a new instance of the SaleModified event.
    /// </summary>
    /// <param name="sale">The modified sale.</param>
    /// <param name="modificationType">The type of modification.</param>
    /// <param name="modificationDetails">Additional details about the modification.</param>
    public SaleModified(Sale sale, string modificationType, string modificationDetails = "")
    {
        SaleId = sale.Id;
        SaleNumber = sale.SaleNumber;
        CustomerId = sale.CustomerId;
        BranchId = sale.BranchId;
        TotalAmount = sale.TotalAmount;
        ItemCount = sale.GetActiveItemCount();
        ModificationType = modificationType;
        ModificationDetails = modificationDetails;
    }
}