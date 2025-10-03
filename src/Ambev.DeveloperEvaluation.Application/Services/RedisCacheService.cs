using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Services;

/// <summary>
/// Redis implementation of the cache service.
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromHours(1);

    /// <summary>
    /// Initializes a new instance of the Redis cache service.
    /// </summary>
    /// <param name="distributedCache">The distributed cache instance.</param>
    /// <param name="logger">Logger instance.</param>
    public RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    /// <inheritdoc/>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(cachedValue))
            {
                return null;
            }

            var result = JsonSerializer.Deserialize<T>(cachedValue, _jsonOptions);
            _logger.LogDebug("Cache hit for key: {Key}", key);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting value from cache for key: {Key}", key);
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var serializedValue = JsonSerializer.Serialize(value, _jsonOptions);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            await _distributedCache.SetStringAsync(key, serializedValue, options, cancellationToken);
            _logger.LogDebug("Cache set for key: {Key} with expiration: {Expiration}", key, expiration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting value in cache for key: {Key}", key);
        }
    }

    /// <inheritdoc/>
    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        return SetAsync(key, value, _defaultExpiration, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            _logger.LogDebug("Cache removed for key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing value from cache for key: {Key}", key);
        }
    }

    /// <inheritdoc/>
    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        // Note: Pattern-based removal is not directly supported by IDistributedCache
        // In a real implementation, you might need to use Redis-specific APIs
        // For this demo, we'll log that pattern removal is not implemented
        _logger.LogWarning("Pattern-based cache removal is not implemented in this Redis cache service. Pattern: {Pattern}", pattern);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            return !string.IsNullOrEmpty(cachedValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if key exists in cache: {Key}", key);
            return false;
        }
    }
}