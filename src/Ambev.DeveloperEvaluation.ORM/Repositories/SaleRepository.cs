using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier including related entities
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale with related entities if found, null otherwise</returns>
    public async Task<Sale?> GetByIdWithRelationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Retrieves sales by customer identifier
    /// </summary>
    /// <param name="customerId">The customer identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of sales for the customer</returns>
    public async Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves sales by branch identifier
    /// </summary>
    /// <param name="branchId">The branch identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of sales for the branch</returns>
    public async Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Where(s => s.BranchId == branchId)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves sales within a date range
    /// </summary>
    /// <param name="startDate">Start date of the range</param>
    /// <param name="endDate">End date of the range</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of sales within the date range</returns>
    public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing sale
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Deletes a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Loads related data for a sale entity
    /// </summary>
    /// <param name="sale">The sale entity to load relations for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task LoadRelatedDataAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Entry(sale)
            .Reference(s => s.Customer)
            .LoadAsync(cancellationToken);

        await _context.Entry(sale)
            .Reference(s => s.Branch)
            .LoadAsync(cancellationToken);

        await _context.Entry(sale)
            .Collection(s => s.Items)
            .LoadAsync(cancellationToken);

        // Load product for each sale item
        foreach (var item in sale.Items)
        {
            await _context.Entry(item)
                .Reference(i => i.Product)
                .LoadAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Gets the total count of sales
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of sales</returns>
    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sales.CountAsync(cancellationToken);
    }

    /// <summary>
    /// Gets paginated sales
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of sales</returns>
    public async Task<IEnumerable<Sale>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .OrderByDescending(s => s.SaleDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}