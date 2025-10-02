using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Product entity operations
/// </summary>
public interface IProductRepository : IBaseRepository<Product>
{

    /// <summary>
    /// Retrieves a product by its name
    /// </summary>
    /// <param name="name">The product name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves products by category
    /// </summary>
    /// <param name="category">The product category</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products in the specified category</returns>
    Task<IEnumerable<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves products that are currently in stock
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products with stock > 0</returns>
    Task<IEnumerable<Product>> GetInStockAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves products with low stock (below threshold)
    /// </summary>
    /// <param name="threshold">Stock threshold</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products with stock below threshold</returns>
    Task<IEnumerable<Product>> GetLowStockAsync(int threshold, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches products by name (partial match)
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products matching the name</returns>
    Task<IEnumerable<Product>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets products within a price range
    /// </summary>
    /// <param name="minPrice">Minimum price</param>
    /// <param name="maxPrice">Maximum price</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products within the price range</returns>
    Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a product is available with the required quantity
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="quantity">Required quantity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if product has sufficient stock, false otherwise</returns>
    Task<bool> IsAvailableAsync(Guid id, int quantity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates product stock quantity
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="quantity">New stock quantity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if stock was updated, false if product not found</returns>
    Task<bool> UpdateStockAsync(Guid id, int quantity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reduces product stock by specified quantity
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="quantity">Quantity to reduce</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if stock was reduced, false if insufficient stock or product not found</returns>
    Task<bool> ReduceStockAsync(Guid id, int quantity, CancellationToken cancellationToken = default);

}