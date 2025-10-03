using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a sale is cancelled.
/// </summary>
public class SaleCancelled : DomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the cancelled sale.
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
    /// Gets the branch ID where the sale belonged.
    /// </summary>
    public Guid BranchId { get; }

    /// <summary>
    /// Gets the original total amount of the sale before cancellation.
    /// </summary>
    public decimal OriginalTotalAmount { get; }

    /// <summary>
    /// Gets the number of items that were in the sale before cancellation.
    /// </summary>
    public int OriginalItemCount { get; }

    /// <summary>
    /// Gets the date when the sale was originally made.
    /// </summary>
    public DateTime OriginalSaleDate { get; }

    /// <summary>
    /// Gets the reason for the cancellation.
    /// </summary>
    public string CancellationReason { get; }

    /// <summary>
    /// Initializes a new instance of the SaleCancelled event.
    /// </summary>
    /// <param name="sale">The cancelled sale.</param>
    /// <param name="originalTotalAmount">The original total amount before cancellation.</param>
    /// <param name="originalItemCount">The original item count before cancellation.</param>
    /// <param name="cancellationReason">The reason for cancellation.</param>
    public SaleCancelled(Sale sale, decimal originalTotalAmount, int originalItemCount, string cancellationReason = "")
    {
        SaleId = sale.Id;
        SaleNumber = sale.SaleNumber;
        CustomerId = sale.CustomerId;
        BranchId = sale.BranchId;
        OriginalTotalAmount = originalTotalAmount;
        OriginalItemCount = originalItemCount;
        OriginalSaleDate = sale.SaleDate;
        CancellationReason = cancellationReason;
    }
}