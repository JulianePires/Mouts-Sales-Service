using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Branch entity operations
/// </summary>
public interface IBranchRepository
{
    /// <summary>
    /// Creates a new branch in the repository
    /// </summary>
    /// <param name="branch">The branch to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created branch</returns>
    Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a branch by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a branch by its name
    /// </summary>
    /// <param name="name">The branch name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    Task<Branch?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves branches by address (partial match)
    /// </summary>
    /// <param name="address">The address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of branches matching the address</returns>
    Task<IEnumerable<Branch>> GetByAddressAsync(string address, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves active branches
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active branches</returns>
    Task<IEnumerable<Branch>> GetActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches branches by name (partial match)
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of branches matching the name</returns>
    Task<IEnumerable<Branch>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a branch is active and operational
    /// </summary>
    /// <param name="id">The branch ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if branch is active, false otherwise</returns>
    Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves branches by manager name
    /// </summary>
    /// <param name="manager">The manager name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of branches with the specified manager</returns>
    Task<IEnumerable<Branch>> GetByManagerAsync(string manager, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing branch
    /// </summary>
    /// <param name="branch">The branch to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated branch</returns>
    Task<Branch> UpdateAsync(Branch branch, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a branch by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the branch to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the branch was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a branch exists by ID
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the branch exists, false otherwise</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of branches
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of branches</returns>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated branches
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of branches</returns>
    Task<IEnumerable<Branch>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}