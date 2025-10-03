using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

/// <summary>
/// Provides seed data for the Product entity
/// Contains diverse product catalog with pricing, categories, and stock information
/// </summary>
public static class ProductSeed
{
    /// <summary>
    /// Configures seed data for products
    /// </summary>
    /// <param name="builder">Entity type builder for Product entity</param>
    public static void Configure(EntityTypeBuilder<Product> builder)
    {
        var products = new List<Product>
        {
            new Product
            {
                Id = Guid.Parse("b1111111-1111-1111-1111-111111111111"),
                Name = "Brahma Lata 350ml",
                Description = "Cerveja Brahma Pilsen em lata de 350ml",
                Category = "Cervejas",
                Price = 3.50m,
                Image = "brahma_lata_350ml.jpg",
                StockQuantity = 500,
                MinStockLevel = 50,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                Name = "Skol Lata 269ml",
                Description = "Cerveja Skol Pilsen em lata de 269ml",
                Category = "Cervejas",
                Price = 2.80m,
                Image = "skol_lata_269ml.jpg",
                StockQuantity = 750,
                MinStockLevel = 75,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b3333333-3333-3333-3333-333333333333"),
                Name = "Antarctica Original Lata 350ml",
                Description = "Cerveja Antarctica Original em lata de 350ml",
                Category = "Cervejas",
                Price = 3.80m,
                Image = "antarctica_lata_350ml.jpg",
                StockQuantity = 300,
                MinStockLevel = 30,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b4444444-4444-4444-4444-444444444444"),
                Name = "Stella Artois Garrafa 550ml",
                Description = "Cerveja Stella Artois Premium em garrafa de 550ml",
                Category = "Cervejas Premium",
                Price = 8.90m,
                Image = "stella_garrafa_550ml.jpg",
                StockQuantity = 200,
                MinStockLevel = 20,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b5555555-5555-5555-5555-555555555555"),
                Name = "Corona Extra Garrafa 355ml",
                Description = "Cerveja Corona Extra mexicana em garrafa de 355ml",
                Category = "Cervejas Importadas",
                Price = 12.50m,
                Image = "corona_garrafa_355ml.jpg",
                StockQuantity = 150,
                MinStockLevel = 15,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b6666666-6666-6666-6666-666666666666"),
                Name = "Guaraná Antarctica Lata 350ml",
                Description = "Refrigerante Guaraná Antarctica em lata de 350ml",
                Category = "Refrigerantes",
                Price = 4.20m,
                Image = "guarana_lata_350ml.jpg",
                StockQuantity = 400,
                MinStockLevel = 40,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b7777777-7777-7777-7777-777777777777"),
                Name = "Pepsi Twist Lata 350ml",
                Description = "Refrigerante Pepsi Twist com limão em lata de 350ml",
                Category = "Refrigerantes",
                Price = 3.90m,
                Image = "pepsi_twist_lata_350ml.jpg",
                StockQuantity = 350,
                MinStockLevel = 35,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b8888888-8888-8888-8888-888888888888"),
                Name = "H2OH! Limão Pet 500ml",
                Description = "Bebida isotônica H2OH! sabor limão em garrafa Pet de 500ml",
                Category = "Isotônicos",
                Price = 5.50m,
                Image = "h2oh_limao_pet_500ml.jpg",
                StockQuantity = 250,
                MinStockLevel = 25,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b9999999-9999-9999-9999-999999999999"),
                Name = "Fusion Pet 500ml",
                Description = "Energético Fusion original em garrafa Pet de 500ml",
                Category = "Energéticos",
                Price = 7.80m,
                Image = "fusion_pet_500ml.jpg",
                StockQuantity = 180,
                MinStockLevel = 18,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("b0000000-0000-0000-0000-000000000000"),
                Name = "Água Crystal Pet 500ml",
                Description = "Água mineral Crystal em garrafa Pet de 500ml",
                Category = "Águas",
                Price = 2.20m,
                Image = "agua_crystal_pet_500ml.jpg",
                StockQuantity = 600,
                MinStockLevel = 60,
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Product
            {
                Id = Guid.Parse("fa000000-0000-0000-0000-000000000000"),
                Name = "Bohemia Pilsen Garrafa 600ml",
                Description = "Cerveja Bohemia Pilsen premium em garrafa de 600ml",
                Category = "Cervejas Premium",
                Price = 9.50m,
                Image = "bohemia_garrafa_600ml.jpg",
                StockQuantity = 120,
                MinStockLevel = 12,
                IsActive = false,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.UtcNow.AddDays(-30)
            }
        };

        builder.HasData(products);
    }
}