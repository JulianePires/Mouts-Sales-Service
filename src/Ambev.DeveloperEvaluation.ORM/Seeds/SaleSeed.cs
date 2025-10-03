using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

/// <summary>
/// Provides seed data for the Sale entity
/// Contains realistic transaction data linking customers, branches, and different sale statuses
/// </summary>
public static class SaleSeed
{
    /// <summary>
    /// Configures seed data for sales
    /// </summary>
    /// <param name="builder">Entity type builder for Sale entity</param>
    public static void Configure(EntityTypeBuilder<Sale> builder)
    {
        var sales = new List<Sale>
        {
            new Sale
            {
                Id = Guid.Parse("e1111111-1111-1111-1111-111111111111"),
                SaleNumber = "SAL-2024-001",
                SaleDate = DateTime.UtcNow.AddDays(-30),
                CustomerId = Guid.Parse("d1111111-1111-1111-1111-111111111111"), // Maria Silva
                BranchId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // São Paulo Downtown
                Status = SaleStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                UpdatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new Sale
            {
                Id = Guid.Parse("e2222222-2222-2222-2222-222222222222"),
                SaleNumber = "SAL-2024-002",
                SaleDate = DateTime.UtcNow.AddDays(-28),
                CustomerId = Guid.Parse("d2222222-2222-2222-2222-222222222222"), // João Santos
                BranchId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Rio de Janeiro Copacabana
                Status = SaleStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-28),
                UpdatedAt = DateTime.UtcNow.AddDays(-28)
            },
            new Sale
            {
                Id = Guid.Parse("e3333333-3333-3333-3333-333333333333"),
                SaleNumber = "SAL-2024-003",
                SaleDate = DateTime.UtcNow.AddDays(-25),
                CustomerId = Guid.Parse("d3333333-3333-3333-3333-333333333333"), // Ana Paula Costa
                BranchId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Belo Horizonte Savassi
                Status = SaleStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-25),
                UpdatedAt = DateTime.UtcNow.AddDays(-25)
            },
            new Sale
            {
                Id = Guid.Parse("e4444444-4444-4444-4444-444444444444"),
                SaleNumber = "SAL-2024-004",
                SaleDate = DateTime.UtcNow.AddDays(-22),
                CustomerId = Guid.Parse("d4444444-4444-4444-4444-444444444444"), // Pedro Oliveira
                BranchId = Guid.Parse("44444444-4444-4444-4444-444444444444"), // Porto Alegre Moinhos
                Status = SaleStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-22),
                UpdatedAt = DateTime.UtcNow.AddDays(-22)
            },
            new Sale
            {
                Id = Guid.Parse("e5555555-5555-5555-5555-555555555555"),
                SaleNumber = "SAL-2024-005",
                SaleDate = DateTime.UtcNow.AddDays(-20),
                CustomerId = Guid.Parse("d5555555-5555-5555-5555-555555555555"), // Carla Mendes
                BranchId = Guid.Parse("55555555-5555-5555-5555-555555555555"), // Brasília Asa Sul
                Status = SaleStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                UpdatedAt = DateTime.UtcNow.AddDays(-20)
            },
            new Sale
            {
                Id = Guid.Parse("e6666666-6666-6666-6666-666666666666"),
                SaleNumber = "SAL-2024-006",
                SaleDate = DateTime.UtcNow.AddDays(-18),
                CustomerId = Guid.Parse("d6666666-6666-6666-6666-666666666666"), // Lucas Ferreira
                BranchId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // São Paulo Downtown
                Status = SaleStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-18),
                UpdatedAt = DateTime.UtcNow.AddDays(-18)
            },
            new Sale
            {
                Id = Guid.Parse("e7777777-7777-7777-7777-777777777777"),
                SaleNumber = "SAL-2024-007",
                SaleDate = DateTime.UtcNow.AddDays(-15),
                CustomerId = Guid.Parse("d7777777-7777-7777-7777-777777777777"), // Juliana Barbosa
                BranchId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Rio de Janeiro Copacabana
                Status = SaleStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                UpdatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new Sale
            {
                Id = Guid.Parse("e8888888-8888-8888-8888-888888888888"),
                SaleNumber = "SAL-2024-008",
                SaleDate = DateTime.UtcNow.AddDays(-12),
                CustomerId = Guid.Parse("d8888888-8888-8888-8888-888888888888"), // Ricardo Almeida
                BranchId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Belo Horizonte Savassi
                Status = SaleStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-12),
                UpdatedAt = DateTime.UtcNow.AddDays(-12)
            },
            new Sale
            {
                Id = Guid.Parse("e9999999-9999-9999-9999-999999999999"),
                SaleNumber = "SAL-2024-009",
                SaleDate = DateTime.UtcNow.AddDays(-10),
                CustomerId = Guid.Parse("d1111111-1111-1111-1111-111111111111"), // Maria Silva
                BranchId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // São Paulo Downtown
                Status = SaleStatus.Draft,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Sale
            {
                Id = Guid.Parse("e0000000-0000-0000-0000-000000000000"),
                SaleNumber = "SAL-2024-010",
                SaleDate = DateTime.UtcNow.AddDays(-8),
                CustomerId = Guid.Parse("d2222222-2222-2222-2222-222222222222"), // João Santos
                BranchId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Rio de Janeiro Copacabana
                Status = SaleStatus.Cancelled,
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                UpdatedAt = DateTime.UtcNow.AddDays(-7)
            }
        };

        builder.HasData(sales);
    }
}