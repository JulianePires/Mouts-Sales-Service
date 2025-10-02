using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICustomerRepository using Entity Framework Core
/// </summary>
public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    /// <summary>
    /// Initializes a new instance of CustomerRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CustomerRepository(DefaultContext context) : base(context)
    {
    }



    /// <summary>
    /// Retrieves a customer by email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    /// <summary>
    /// Retrieves a customer by phone number
    /// </summary>
    /// <param name="phone">The phone number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    public async Task<Customer?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be null or empty.", nameof(phone));

        return await _dbSet
            .FirstOrDefaultAsync(c => c.Phone == phone, cancellationToken);
    }

    /// <summary>
    /// Retrieves customers by active status
    /// </summary>
    /// <param name="isActive">The customer active status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers with the specified status</returns>
    public async Task<IEnumerable<Customer>> GetByStatusAsync(bool isActive, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.IsActive == isActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Searches customers by name (partial match)
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers matching the name</returns>
    public async Task<IEnumerable<Customer>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Enumerable.Empty<Customer>();

        return await _dbSet
            .Where(c => c.Name.Contains(name))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets customers within an age range
    /// </summary>
    /// <param name="minAge">Minimum age</param>
    /// <param name="maxAge">Maximum age</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers within the age range</returns>
    public async Task<IEnumerable<Customer>> GetByAgeRangeAsync(int minAge, int maxAge, CancellationToken cancellationToken = default)
    {
        if (minAge < 0)
            throw new ArgumentOutOfRangeException(nameof(minAge), "Minimum age must be non-negative.");
        if (maxAge < minAge)
            throw new ArgumentOutOfRangeException(nameof(maxAge), "Maximum age must be greater than or equal to minimum age.");

        var maxBirthDate = DateTime.Now.AddYears(-minAge);
        var minBirthDate = DateTime.Now.AddYears(-maxAge - 1);

        return await _dbSet
            .Where(c => c.BirthDate >= minBirthDate && c.BirthDate <= maxBirthDate)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

}