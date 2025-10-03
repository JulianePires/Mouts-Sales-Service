namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Interface for caching operations.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Gets a cached value by key.
    /// </summary>
    /// <typeparam name="T">The type of the cached value.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The cached value or default if not found.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Sets a value in cache with expiration.
    /// </summary>
    /// <typeparam name="T">The type of the value to cache.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="expiration">The expiration time.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Sets a value in cache with default expiration.
    /// </summary>
    /// <typeparam name="T">The type of the value to cache.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Removes a value from cache.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes multiple values from cache by pattern.
    /// </summary>
    /// <param name="pattern">The key pattern to match.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a key exists in cache.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>True if the key exists, false otherwise.</returns>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}