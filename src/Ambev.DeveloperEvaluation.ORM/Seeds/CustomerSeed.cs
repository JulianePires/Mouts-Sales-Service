using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

/// <summary>
/// Provides seed data for the Customer entity
/// Contains diverse customer profiles with realistic personal and contact information
/// </summary>
public static class CustomerSeed
{
    /// <summary>
    /// Configures seed data for customers
    /// </summary>
    /// <param name="builder">Entity type builder for Customer entity</param>
    public static void Configure(EntityTypeBuilder<Customer> builder)
    {
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = Guid.Parse("d1111111-1111-1111-1111-111111111111"),
                Name = "Maria Silva",
                Email = "maria.silva@email.com",
                Phone = "+55 11 99876-5432",
                Address = "Rua das Flores, 123 - Vila Madalena, São Paulo - SP",
                BirthDate = DateTime.SpecifyKind(new DateTime(1985, 3, 15), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 2, 3, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 2, 3, 10, 0, 0), DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("d2222222-2222-2222-2222-222222222222"),
                Name = "João Santos",
                Email = "joao.santos@email.com",
                Phone = "+55 21 98765-4321",
                Address = "Av. Atlântica, 456 - Copacabana, Rio de Janeiro - RJ",
                BirthDate = DateTime.SpecifyKind(new DateTime(1990, 7, 22), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 3, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 3, 10, 0, 0), DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("d3333333-3333-3333-3333-333333333333"),
                Name = "Ana Paula Costa",
                Email = "ana.costa@email.com",
                Phone = "+55 31 97654-3210",
                Address = "Rua Pampulha, 789 - Savassi, Belo Horizonte - MG",
                BirthDate = DateTime.SpecifyKind(new DateTime(1988, 11, 8), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                Name = "Pedro Oliveira",
                Email = "pedro.oliveira@email.com",
                Phone = "+55 51 96543-2109",
                Address = "Rua dos Andradas, 321 - Centro Histórico, Porto Alegre - RS",
                BirthDate = DateTime.SpecifyKind(new DateTime(1992, 5, 30), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("d5555555-5555-5555-5555-555555555555"),
                Name = "Carla Mendes",
                Email = "carla.mendes@email.com",
                Phone = "+55 61 95432-1098",
                Address = "SHIS QI 15, Conjunto 12, Casa 5 - Lago Sul, Brasília - DF",
                BirthDate = DateTime.SpecifyKind(new DateTime(1987, 9, 14), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("d6666666-6666-6666-6666-666666666666"),
                Name = "Lucas Ferreira",
                Email = "lucas.ferreira@email.com",
                Phone = "+55 11 94321-0987",
                Address = "Alameda Santos, 654 - Jardins, São Paulo - SP",
                BirthDate = DateTime.SpecifyKind(new DateTime(1995, 12, 3), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("d7777777-7777-7777-7777-777777777777"),
                Name = "Juliana Barbosa",
                Email = "juliana.barbosa@email.com",
                Phone = "+55 21 93210-9876",
                Address = "Rua Visconde de Pirajá, 987 - Ipanema, Rio de Janeiro - RJ",
                BirthDate = DateTime.SpecifyKind(new DateTime(1993, 4, 18), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new Customer
            {
                Id = Guid.Parse("d8888888-8888-8888-8888-888888888888"),
                Name = "Ricardo Almeida",
                Email = "ricardo.almeida@email.com",
                Phone = "+55 31 92109-8765",
                Address = "Av. Afonso Pena, 234 - Centro, Belo Horizonte - MG",
                BirthDate = DateTime.SpecifyKind(new DateTime(1989, 8, 25), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            }
        };

        builder.HasData(customers);
    }
}