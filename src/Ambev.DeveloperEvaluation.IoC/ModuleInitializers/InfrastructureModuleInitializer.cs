using Ambev.DeveloperEvaluation.Application.Common.Services;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Application.EventHandlers;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());

        // Auto-register all repositories using assembly scanning
        RegisterRepositories(builder.Services);

        // Register the discount service
        builder.Services.AddScoped<IDiscountService, QuantityDiscountService>();

        // Register application services
        builder.Services.AddScoped<ISaleService, Ambev.DeveloperEvaluation.Application.Common.Services.SaleService>();

        // Register event services
        builder.Services.AddSingleton<IEventPublisher, AzureServiceBusEventPublisher>();
        builder.Services.AddScoped<ICacheService, RedisCacheService>();
        builder.Services.AddScoped<IDomainEventService, DomainEventService>();

        // Register event handlers
        builder.Services.AddScoped<IDomainEventHandler<SaleCreated>, SaleCreatedEventHandler>();
        builder.Services.AddScoped<IDomainEventHandler<SaleModified>, SaleModifiedEventHandler>();
        builder.Services.AddScoped<IDomainEventHandler<SaleCancelled>, SaleCancelledEventHandler>();
        builder.Services.AddScoped<IDomainEventHandler<ItemCancelled>, ItemCancelledEventHandler>();

        // Register Redis caching (if connection string is provided)
        var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            try
            {
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                    options.InstanceName = builder.Configuration["Redis:InstanceName"] ?? "AmbevDeveloperEvaluation";
                });
            }
            catch
            {
                // If Redis is not available, fall back to in-memory cache
                builder.Services.AddMemoryCache();
            }
        }
        else
        {
            // Fall back to in-memory cache if Redis is not configured
            builder.Services.AddMemoryCache();
        }
    }

    /// <summary>
    /// Automatically registers all repository implementations using convention-based assembly scanning.
    /// Scans the ORM assembly for classes that implement repository interfaces.
    /// </summary>
    /// <param name="services">The service collection to register repositories with</param>
    private static void RegisterRepositories(IServiceCollection services)
    {
        // Get the ORM assembly where repository implementations are located
        var ormAssembly = Assembly.GetAssembly(typeof(DefaultContext))!;

        // Get all repository interface types from the Domain assembly
        var domainAssembly = Assembly.GetAssembly(typeof(IBaseRepository<>))!;
        var repositoryInterfaceTypes = domainAssembly.GetTypes()
            .Where(t => t.IsInterface &&
                       t.Name.EndsWith("Repository") &&
                       t.Name.StartsWith("I"))
            .ToList();

        // Find and register implementations
        foreach (var interfaceType in repositoryInterfaceTypes)
        {
            // Find the concrete implementation in the ORM assembly
            var implementationType = ormAssembly.GetTypes()
                .FirstOrDefault(t => t.IsClass &&
                               !t.IsAbstract &&
                               interfaceType.IsAssignableFrom(t));

            if (implementationType != null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}