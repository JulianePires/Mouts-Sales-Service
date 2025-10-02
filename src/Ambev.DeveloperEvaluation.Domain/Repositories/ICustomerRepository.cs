using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Customer entity operations
/// </summary>
public interface ICustomerRepository : IBaseRepository<Customer>
{

    /// <summary>
    /// Retrieves a customer by email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a customer by phone number
    /// </summary>
    /// <param name="phone">The phone number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    Task<Customer?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves customers by active status
    /// </summary>
    /// <param name="isActive">The customer active status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers with the specified status</returns>
    Task<IEnumerable<Customer>> GetByStatusAsync(bool isActive, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches customers by name (partial match)
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers matching the name</returns>
    Task<IEnumerable<Customer>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets customers within an age range
    /// </summary>
    /// <param name="minAge">Minimum age</param>
    /// <param name="maxAge">Maximum age</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers within the age range</returns>
    Task<IEnumerable<Customer>> GetByAgeRangeAsync(int minAge, int maxAge, CancellationToken cancellationToken = default);

}