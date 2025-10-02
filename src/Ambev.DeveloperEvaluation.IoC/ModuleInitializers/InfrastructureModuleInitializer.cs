using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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