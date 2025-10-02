using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IBranchRepository using Entity Framework Core
/// </summary>
public class BranchRepository : BaseRepository<Branch>, IBranchRepository
{

    /// <summary>
    /// Initializes a new instance of BranchRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public BranchRepository(DefaultContext context) : base(context)
    {
    }

    /// <summary>
    /// Creates a new branch in the repository
    /// </summary>
    /// <param name="branch">The branch to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created branch</returns>
    public async Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(branch);
        _dbSet.Add(branch);
        await _context.SaveChangesAsync(cancellationToken);
        return branch;
    }

    /// <summary>
    /// Retrieves a branch by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a branch by its name
    /// </summary>
    /// <param name="name">The branch name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    public async Task<Branch?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Branch name cannot be null or empty.", nameof(name));

        return await _dbSet
            .FirstOrDefaultAsync(b => b.Name == name, cancellationToken);
    }

    /// <summary>
    /// Retrieves branches by address (partial match)
    /// </summary>
    /// <param name="address">The address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of branches matching the address</returns>
    public async Task<IEnumerable<Branch>> GetByAddressAsync(string address, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(address))
            return Enumerable.Empty<Branch>();

        return await _dbSet
            .Where(b => b.Address.Contains(address))
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves branches by manager name
    /// </summary>
    /// <param name="manager">The manager name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of branches with the specified manager</returns>
    public async Task<IEnumerable<Branch>> GetByManagerAsync(string manager, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(manager))
            throw new ArgumentException("Manager name cannot be null or empty.", nameof(manager));

        return await _dbSet
            .Where(b => b.Manager == manager)
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves active branches
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active branches</returns>
    public async Task<IEnumerable<Branch>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => b.IsActive)
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Searches branches by name (partial match)
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of branches matching the name</returns>
    public async Task<IEnumerable<Branch>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Enumerable.Empty<Branch>();

        return await _dbSet
            .Where(b => b.Name.Contains(name))
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Checks if a branch is active and operational
    /// </summary>
    /// <param name="id">The branch ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if branch is active, false otherwise</returns>
    public async Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var branch = await GetByIdAsync(id, cancellationToken);
        return branch != null && branch.IsActive;
    }



    /// <summary>
    /// Updates an existing branch
    /// </summary>
    /// <param name="branch">The branch to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated branch</returns>
    public async Task<Branch> UpdateAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);
        return branch;
    }

    /// <summary>
    /// Deletes a branch by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the branch to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the branch was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var branch = await GetByIdAsync(id, cancellationToken);
        if (branch == null)
            return false;

        _dbSet.Remove(branch);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Checks if a branch exists by ID
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the branch exists, false otherwise</returns>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(b => b.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets the total count of branches
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of branches</returns>
    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    /// <summary>
    /// Gets paginated branches
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of branches</returns>
    public async Task<IEnumerable<Branch>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        return await _dbSet
            .OrderBy(b => b.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}