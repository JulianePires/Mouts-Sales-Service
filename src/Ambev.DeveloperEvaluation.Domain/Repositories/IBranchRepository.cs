using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Branch entity operations
/// </summary>
public interface IBranchRepository : IBaseRepository<Branch>
{

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

}