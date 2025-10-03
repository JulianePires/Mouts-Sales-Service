using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

/// <summary>
/// Provides seed data for the Branch entity
/// Contains diverse branch locations with realistic management information
/// </summary>
public static class BranchSeed
{
    /// <summary>
    /// Configures seed data for branches
    /// </summary>
    /// <param name="builder">Entity type builder for Branch entity</param>
    public static void Configure(EntityTypeBuilder<Branch> builder)
    {
        var branches = new List<Branch>
        {
            new Branch
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "São Paulo Downtown",
                Address = "Av. Paulista, 1234 - Bela Vista, São Paulo - SP, 01310-100",
                Phone = "+55 11 3456-7890",
                Email = "saopaulo.downtown@ambev.com",
                Manager = "Carlos Silva",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 4, 3, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 4, 3, 10, 0, 0), DateTimeKind.Utc)
            },
            new Branch
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Rio de Janeiro Copacabana",
                Address = "Av. Nossa Senhora de Copacabana, 987 - Copacabana, Rio de Janeiro - RJ, 22070-012",
                Phone = "+55 21 2987-6543",
                Email = "rio.copacabana@ambev.com",
                Manager = "Ana Costa",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 5, 3, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 5, 3, 10, 0, 0), DateTimeKind.Utc)
            },
            new Branch
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Belo Horizonte Savassi",
                Address = "Rua da Bahia, 456 - Savassi, Belo Horizonte - MG, 30160-012",
                Phone = "+55 31 3321-9876",
                Email = "bh.savassi@ambev.com",
                Manager = "Roberto Santos",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 6, 3, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 6, 3, 10, 0, 0), DateTimeKind.Utc)
            },
            new Branch
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Porto Alegre Moinhos",
                Address = "Rua Padre Chagas, 789 - Moinhos de Vento, Porto Alegre - RS, 90570-080",
                Phone = "+55 51 3234-5678",
                Email = "poa.moinhos@ambev.com",
                Manager = "Fernanda Lima",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 7, 3, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 7, 3, 10, 0, 0), DateTimeKind.Utc)
            },
            new Branch
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Brasília Asa Sul",
                Address = "SCS Quadra 02, Bloco A, Loja 15 - Asa Sul, Brasília - DF, 70318-900",
                Phone = "+55 61 3445-6789",
                Email = "brasilia.asasul@ambev.com",
                Manager = "João Oliveira",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 8, 3, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 8, 3, 10, 0, 0), DateTimeKind.Utc)
            }
        };

        builder.HasData(branches);
    }
}