using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICustomerRepository using Entity Framework Core
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of CustomerRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CustomerRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new customer in the repository
    /// </summary>
    /// <param name="customer">The customer to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created customer</returns>
    public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(customer);
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return customer;
    }

    /// <summary>
    /// Retrieves a customer by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the customer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
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

        return await _context.Customers
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

        return await _context.Customers
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
        return await _context.Customers
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

        return await _context.Customers
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

        return await _context.Customers
            .Where(c => c.BirthDate >= minBirthDate && c.BirthDate <= maxBirthDate)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing customer
    /// </summary>
    /// <param name="customer">The customer to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated customer</returns>
    public async Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return customer;
    }

    /// <summary>
    /// Deletes a customer by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the customer was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await GetByIdAsync(id, cancellationToken);
        if (customer == null)
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Checks if a customer exists by ID
    /// </summary>
    /// <param name="id">The unique identifier of the customer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the customer exists, false otherwise</returns>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AnyAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets the total count of customers
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of customers</returns>
    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Customers.CountAsync(cancellationToken);
    }

    /// <summary>
    /// Gets paginated customers
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of customers</returns>
    public async Task<IEnumerable<Customer>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        return await _context.Customers
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}