using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a sale is created.
/// </summary>
public class SaleCreated : DomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the created sale.
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
    /// Gets the branch ID where the sale was created.
    /// </summary>
    public Guid BranchId { get; }

    /// <summary>
    /// Gets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// Gets the number of items in the sale.
    /// </summary>
    public int ItemCount { get; }

    /// <summary>
    /// Gets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; }

    /// <summary>
    /// Initializes a new instance of the SaleCreated event.
    /// </summary>
    /// <param name="sale">The created sale.</param>
    public SaleCreated(Sale sale)
    {
        SaleId = sale.Id;
        SaleNumber = sale.SaleNumber;
        CustomerId = sale.CustomerId;
        BranchId = sale.BranchId;
        TotalAmount = sale.TotalAmount;
        ItemCount = sale.GetActiveItemCount();
        SaleDate = sale.SaleDate;
    }
}