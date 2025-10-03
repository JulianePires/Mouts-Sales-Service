using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM;

public class DefaultContext : DbContext
{
    private readonly IServiceProvider? _serviceProvider;

    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Product> Products { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options, IServiceProvider? serviceProvider = null) : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Ignore domain events as they are not persisted entities
        modelBuilder.Ignore<Domain.Events.DomainEvent>();

        // Ignore specific domain event implementations
        modelBuilder.Ignore<Domain.Events.SaleCreated>();
        modelBuilder.Ignore<Domain.Events.SaleModified>();
        modelBuilder.Ignore<Domain.Events.SaleCancelled>();
        modelBuilder.Ignore<Domain.Events.ItemCancelled>();
        modelBuilder.Ignore<Domain.Events.UserRegisteredEvent>();

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect domain events before saving
        var domainEvents = new List<Domain.Events.DomainEvent>();

        var aggregateRoots = ChangeTracker.Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        foreach (var aggregateRoot in aggregateRoots)
        {
            domainEvents.AddRange(aggregateRoot.DomainEvents);
            aggregateRoot.ClearDomainEvents();
        }

        // Save changes first
        var result = await base.SaveChangesAsync(cancellationToken);

        // Process domain events after successful save
        if (domainEvents.Any() && _serviceProvider != null)
        {
            try
            {
                var domainEventService = _serviceProvider.GetService<IDomainEventService>();
                if (domainEventService != null)
                {
                    await domainEventService.ProcessEventsAsync(domainEvents, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the transaction
                // In production, you might want to use a more sophisticated error handling
                Console.WriteLine($"Error processing domain events: {ex.Message}");
            }
        }

        return result;
    }
}
public class YourDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    public DefaultContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DefaultContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
               connectionString,
               b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
        );

        return new DefaultContext(builder.Options);
    }
}