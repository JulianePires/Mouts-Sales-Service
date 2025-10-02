using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Unit.ORM.Repositories;

/// <summary>
/// Base test fixture that provides common in-memory database setup for repository tests.
/// This class eliminates code duplication across repository test classes.
/// </summary>
/// <typeparam name="TRepository">The repository type being tested</typeparam>
public abstract class BaseRepositoryTestFixture<TRepository> : IDisposable where TRepository : class
{
    protected readonly DefaultContext Context;
    protected readonly TRepository Repository;

    /// <summary>
    /// Initializes a new instance of BaseRepositoryTestFixture
    /// </summary>
    protected BaseRepositoryTestFixture()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new DefaultContext(options);
        Repository = CreateRepository(Context);
    }

    /// <summary>
    /// Creates an instance of the repository being tested.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <param name="context">The database context to use</param>
    /// <returns>An instance of the repository</returns>
    protected abstract TRepository CreateRepository(DefaultContext context);

    /// <summary>
    /// Saves changes to the database context.
    /// Helper method for tests that need to persist changes.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The number of state entries written to the database</returns>
    protected async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Clears all change tracking from the context and reloads entities from the database.
    /// Useful for testing scenarios where you need to ensure entities are loaded fresh.
    /// </summary>
    protected void ClearChangeTracker()
    {
        Context.ChangeTracker.Clear();
    }

    /// <summary>
    /// Disposes of the database context and cleans up resources
    /// </summary>
    public void Dispose()
    {
        Context?.Dispose();
        GC.SuppressFinalize(this);
    }
}