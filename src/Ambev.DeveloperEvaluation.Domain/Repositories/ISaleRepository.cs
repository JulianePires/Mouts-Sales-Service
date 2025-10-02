using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its unique identifier including related entities
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale with related entities if found, null otherwise</returns>
    Task<Sale?> GetByIdWithRelationsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves sales by customer identifier
    /// </summary>
    /// <param name="customerId">The customer identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of sales for the customer</returns>
    Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves sales by branch identifier
    /// </summary>
    /// <param name="branchId">The branch identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of sales for the branch</returns>
    Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves sales within a date range
    /// </summary>
    /// <param name="startDate">Start date of the range</param>
    /// <param name="endDate">End date of the range</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of sales within the date range</returns>
    Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads related data for a sale entity
    /// </summary>
    /// <param name="sale">The sale entity to load relations for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task LoadRelatedDataAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of sales
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of sales</returns>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated sales
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of sales</returns>
    Task<IEnumerable<Sale>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}