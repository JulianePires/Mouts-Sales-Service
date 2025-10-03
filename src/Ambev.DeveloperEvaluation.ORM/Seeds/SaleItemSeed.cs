using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

/// <summary>
/// Provides seed data for the SaleItem entity
/// Contains product quantities, pricing, and discount calculations for each sale
/// </summary>
public static class SaleItemSeed
{
    /// <summary>
    /// Configures seed data for sale items using anonymous objects to handle shadow properties
    /// </summary>
    /// <param name="builder">Entity type builder for SaleItem entity</param>
    public static void Configure<T>(EntityTypeBuilder<T> builder) where T : class
    {
        var saleItems = new[]
        {
            // Sale 1: Maria Silva - SAL-2024-001
            new
            {
                Id = Guid.Parse("f1111111-1111-1111-1111-111111111111"),
                SaleId = Guid.Parse("e1111111-1111-1111-1111-111111111111"),
                ProductId = Guid.Parse("b1111111-1111-1111-1111-111111111111"), // Brahma Lata 350ml
                Quantity = 6, // 6 units = 10% discount
                DiscountPercent = 10m,
                UnitPrice = 3.50m,
                TotalPrice = 18.90m, // 6 * 3.50 * 0.90
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("f2222222-2222-2222-2222-222222222222"),
                SaleId = Guid.Parse("e1111111-1111-1111-1111-111111111111"),
                ProductId = Guid.Parse("b6666666-6666-6666-6666-666666666666"), // Guaraná Antarctica Lata 350ml
                Quantity = 2,
                DiscountPercent = 0m,
                UnitPrice = 4.20m,
                TotalPrice = 8.40m, // 2 * 4.20 (no discount)
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                UpdatedAt = (DateTime?)null
            },

            // Sale 2: João Santos - SAL-2024-002
            new
            {
                Id = Guid.Parse("f3333333-3333-3333-3333-333333333333"),
                SaleId = Guid.Parse("e2222222-2222-2222-2222-222222222222"),
                ProductId = Guid.Parse("b2222222-2222-2222-2222-222222222222"), // Skol Lata 269ml
                Quantity = 12, // 12 units = 20% discount
                DiscountPercent = 20m,
                UnitPrice = 2.80m,
                TotalPrice = 26.88m, // 12 * 2.80 * 0.80
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-28),
                UpdatedAt = (DateTime?)null
            },

            // Sale 3: Ana Paula Costa - SAL-2024-003
            new
            {
                Id = Guid.Parse("f4444444-4444-4444-4444-444444444444"),
                SaleId = Guid.Parse("e3333333-3333-3333-3333-333333333333"),
                ProductId = Guid.Parse("b4444444-4444-4444-4444-444444444444"), // Stella Artois Garrafa 550ml
                Quantity = 4, // 4 units = 10% discount
                DiscountPercent = 10m,
                UnitPrice = 8.90m,
                TotalPrice = 32.04m, // 4 * 8.90 * 0.90
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-25),
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("f5555555-5555-5555-5555-555555555555"),
                SaleId = Guid.Parse("e3333333-3333-3333-3333-333333333333"),
                ProductId = Guid.Parse("b8888888-8888-8888-8888-888888888888"), // H2OH! Limão Pet 500ml
                Quantity = 3,
                DiscountPercent = 0m,
                UnitPrice = 5.50m,
                TotalPrice = 16.50m, // 3 * 5.50 (no discount)
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-25),
                UpdatedAt = (DateTime?)null
            },

            // Sale 4: Pedro Oliveira - SAL-2024-004
            new
            {
                Id = Guid.Parse("f6666666-6666-6666-6666-666666666666"),
                SaleId = Guid.Parse("e4444444-4444-4444-4444-444444444444"),
                ProductId = Guid.Parse("b5555555-5555-5555-5555-555555555555"), // Corona Extra Garrafa 355ml
                Quantity = 20, // 20 units = 20% discount
                DiscountPercent = 20m,
                UnitPrice = 12.50m,
                TotalPrice = 200.00m, // 20 * 12.50 * 0.80
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-22),
                UpdatedAt = (DateTime?)null
            },

            // Sale 5: Carla Mendes - SAL-2024-005
            new
            {
                Id = Guid.Parse("f7777777-7777-7777-7777-777777777777"),
                SaleId = Guid.Parse("e5555555-5555-5555-5555-555555555555"),
                ProductId = Guid.Parse("b0000000-0000-0000-0000-000000000000"), // Água Crystal Pet 500ml
                Quantity = 10, // 10 units = 20% discount
                DiscountPercent = 20m,
                UnitPrice = 2.20m,
                TotalPrice = 17.60m, // 10 * 2.20 * 0.80
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("f8888888-8888-8888-8888-888888888888"),
                SaleId = Guid.Parse("e5555555-5555-5555-5555-555555555555"),
                ProductId = Guid.Parse("b9999999-9999-9999-9999-999999999999"), // Fusion Pet 500ml
                Quantity = 5, // 5 units = 10% discount
                DiscountPercent = 10m,
                UnitPrice = 7.80m,
                TotalPrice = 35.10m, // 5 * 7.80 * 0.90
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                UpdatedAt = (DateTime?)null
            },

            // Sale 6: Lucas Ferreira - SAL-2024-006
            new
            {
                Id = Guid.Parse("f9999999-9999-9999-9999-999999999999"),
                SaleId = Guid.Parse("e6666666-6666-6666-6666-666666666666"),
                ProductId = Guid.Parse("b3333333-3333-3333-3333-333333333333"), // Antarctica Original Lata 350ml
                Quantity = 8, // 8 units = 10% discount
                DiscountPercent = 10m,
                UnitPrice = 3.80m,
                TotalPrice = 27.36m, // 8 * 3.80 * 0.90
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-18),
                UpdatedAt = (DateTime?)null
            },

            // Sale 7: Juliana Barbosa - SAL-2024-007
            new
            {
                Id = Guid.Parse("f0000000-0000-0000-0000-000000000000"),
                SaleId = Guid.Parse("e7777777-7777-7777-7777-777777777777"),
                ProductId = Guid.Parse("b7777777-7777-7777-7777-777777777777"), // Pepsi Twist Lata 350ml
                Quantity = 15, // 15 units = 20% discount
                DiscountPercent = 20m,
                UnitPrice = 3.90m,
                TotalPrice = 46.80m, // 15 * 3.90 * 0.80
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                UpdatedAt = (DateTime?)null
            },

            // Sale 8: Ricardo Almeida - SAL-2024-008
            new
            {
                Id = Guid.Parse("fa000000-0000-0000-0000-000000000000"),
                SaleId = Guid.Parse("e8888888-8888-8888-8888-888888888888"),
                ProductId = Guid.Parse("b1111111-1111-1111-1111-111111111111"), // Brahma Lata 350ml
                Quantity = 1,
                DiscountPercent = 0m,
                UnitPrice = 3.50m,
                TotalPrice = 3.50m, // 1 * 3.50 (no discount)
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-12),
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("fb000000-0000-0000-0000-000000000000"),
                SaleId = Guid.Parse("e8888888-8888-8888-8888-888888888888"),
                ProductId = Guid.Parse("b2222222-2222-2222-2222-222222222222"), // Skol Lata 269ml
                Quantity = 1,
                DiscountPercent = 0m,
                UnitPrice = 2.80m,
                TotalPrice = 2.80m, // 1 * 2.80 (no discount)
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-12),
                UpdatedAt = (DateTime?)null
            },

            // Sale 9: Maria Silva - SAL-2024-009 (Draft status)
            new
            {
                Id = Guid.Parse("fc000000-0000-0000-0000-000000000000"),
                SaleId = Guid.Parse("e9999999-9999-9999-9999-999999999999"),
                ProductId = Guid.Parse("b6666666-6666-6666-6666-666666666666"), // Guaraná Antarctica Lata 350ml
                Quantity = 3,
                DiscountPercent = 0m,
                UnitPrice = 4.20m,
                TotalPrice = 12.60m, // 3 * 4.20 (no discount)
                IsCancelled = false,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = (DateTime?)null
            }

            // Note: Sale 10 (SAL-2024-010) is cancelled, so no sale items
        };

        builder.HasData(saleItems);
    }
}