using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IProductRepository using Entity Framework Core
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of ProductRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new product in the repository
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product</returns>
    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    /// <summary>
    /// Retrieves a product by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a product by its name
    /// </summary>
    /// <param name="name">The product name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be null or empty.", nameof(name));

        return await _context.Products
            .FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }

    /// <summary>
    /// Retrieves products by category
    /// </summary>
    /// <param name="category">The product category</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products in the specified category</returns>
    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be null or empty.", nameof(category));

        return await _context.Products
            .Where(p => p.Category == category)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves products that are currently in stock
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products with stock > 0</returns>
    public async Task<IEnumerable<Product>> GetInStockAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => p.StockQuantity > 0)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves products with low stock (below threshold)
    /// </summary>
    /// <param name="threshold">Stock threshold</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products with stock below threshold</returns>
    public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold, CancellationToken cancellationToken = default)
    {
        if (threshold < 0)
            throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be non-negative.");

        return await _context.Products
            .Where(p => p.StockQuantity < threshold && p.StockQuantity >= 0)
            .OrderBy(p => p.StockQuantity)
            .ThenBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Searches products by name (partial match)
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products matching the name</returns>
    public async Task<IEnumerable<Product>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Enumerable.Empty<Product>();

        return await _context.Products
            .Where(p => p.Name.Contains(name))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets products within a price range
    /// </summary>
    /// <param name="minPrice">Minimum price</param>
    /// <param name="maxPrice">Maximum price</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products within the price range</returns>
    public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
    {
        if (minPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(minPrice), "Minimum price must be non-negative.");
        if (maxPrice < minPrice)
            throw new ArgumentOutOfRangeException(nameof(maxPrice), "Maximum price must be greater than or equal to minimum price.");

        return await _context.Products
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .OrderBy(p => p.Price)
            .ThenBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Checks if a product is available with the required quantity
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="quantity">Required quantity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if product has sufficient stock, false otherwise</returns>
    public async Task<bool> IsAvailableAsync(Guid id, int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
            return false;

        var product = await GetByIdAsync(id, cancellationToken);
        return product != null && product.StockQuantity >= quantity;
    }

    /// <summary>
    /// Updates product stock quantity
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="quantity">New stock quantity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if stock was updated, false if product not found</returns>
    public async Task<bool> UpdateStockAsync(Guid id, int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Stock quantity must be non-negative.");

        var product = await GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        product.StockQuantity = quantity;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Reduces product stock by specified quantity
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="quantity">Quantity to reduce</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if stock was reduced, false if insufficient stock or product not found</returns>
    public async Task<bool> ReduceStockAsync(Guid id, int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity to reduce must be positive.");

        var product = await GetByIdAsync(id, cancellationToken);
        if (product == null || product.StockQuantity < quantity)
            return false;

        product.StockQuantity -= quantity;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="product">The product to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product</returns>
    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    /// <summary>
    /// Deletes a product by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Checks if a product exists by ID
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product exists, false otherwise</returns>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets the total count of products
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Total number of products</returns>
    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.CountAsync(cancellationToken);
    }

    /// <summary>
    /// Gets paginated products
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products</returns>
    public async Task<IEnumerable<Product>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        return await _context.Products
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}