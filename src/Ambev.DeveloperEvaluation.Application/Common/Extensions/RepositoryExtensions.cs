using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Common.Extensions;

/// <summary>
/// Extension methods for repository operations to reduce boilerplate code.
/// </summary>
public static class RepositoryExtensions
{
    /// <summary>
    /// Gets an entity by ID and throws an exception if not found.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="repository">The repository instance.</param>
    /// <param name="id">The entity ID.</param>
    /// <param name="paramName">The parameter name for the exception.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The entity if found.</returns>
    /// <exception cref="ArgumentException">Thrown when entity is not found.</exception>
    public static async Task<TEntity> GetOrThrowAsync<TEntity>(
        this IBaseRepository<TEntity> repository,
        Guid id,
        string paramName,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            throw new ArgumentException($"{typeof(TEntity).Name} with ID {id} not found.", paramName);
        }
        return entity;
    }
}