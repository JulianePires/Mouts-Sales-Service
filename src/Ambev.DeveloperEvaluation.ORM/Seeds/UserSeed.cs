using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

/// <summary>
/// Provides seed data for the User entity
/// Contains users with different roles (Admin, Manager, Customer) and authentication data
/// </summary>
public static class UserSeed
{
    /// <summary>
    /// Configures seed data for users
    /// </summary>
    /// <param name="builder">Entity type builder for User entity</param>
    public static void Configure(EntityTypeBuilder<User> builder)
    {
        var users = new List<User>
        {
            new User
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                Username = "admin",
                Email = "admin@ambev.com",
                Phone = "+55 11 99999-9999",
                Password = "admin123", // In production, this should be hashed
                Role = UserRole.Admin,
                Status = UserStatus.Active,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                Username = "manager.carlos",
                Email = "carlos.silva@ambev.com",
                Phone = "+55 11 98888-8888",
                Password = "manager123",
                Role = UserRole.Manager,
                Status = UserStatus.Active,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                Username = "manager.ana",
                Email = "ana.costa@ambev.com",
                Phone = "+55 21 97777-7777",
                Password = "manager123",
                Role = UserRole.Manager,
                Status = UserStatus.Active,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("a4444444-4444-4444-4444-444444444444"),
                Username = "manager.roberto",
                Email = "roberto.santos@ambev.com",
                Phone = "+55 31 96666-6666",
                Password = "manager123",
                Role = UserRole.Manager,
                Status = UserStatus.Active,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("a5555555-5555-5555-5555-555555555555"),
                Username = "manager.fernanda",
                Email = "fernanda.lima@ambev.com",
                Phone = "+55 51 95555-5555",
                Password = "manager123",
                Role = UserRole.Manager,
                Status = UserStatus.Active,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("a6666666-6666-6666-6666-666666666666"),
                Username = "manager.joao",
                Email = "joao.oliveira@ambev.com",
                Phone = "+55 61 94444-4444",
                Password = "manager123",
                Role = UserRole.Manager,
                Status = UserStatus.Active,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("a7777777-7777-7777-7777-777777777777"),
                Username = "customer.maria",
                Email = "maria.silva@email.com",
                Phone = "+55 11 99876-5432",
                Password = "customer123",
                Role = UserRole.Customer,
                Status = UserStatus.Active,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("a8888888-8888-8888-8888-888888888888"),
                Username = "customer.joao",
                Email = "joao.santos@email.com",
                Phone = "+55 21 98765-4321",
                Password = "customer123",
                Role = UserRole.Customer,
                Status = UserStatus.Active,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("a9999999-9999-9999-9999-999999999999"),
                Username = "customer.ana",
                Email = "ana.costa@email.com",
                Phone = "+55 31 97654-3210",
                Password = "customer123",
                Role = UserRole.Customer,
                Status = UserStatus.Inactive,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new User
            {
                Id = Guid.Parse("a0000000-0000-0000-0000-000000000000"),
                Username = "customer.pedro",
                Email = "pedro.oliveira@email.com",
                Phone = "+55 51 96543-2109",
                Password = "customer123",
                Role = UserRole.Customer,
                Status = UserStatus.Suspended,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 10, 0, 0), DateTimeKind.Utc),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            }
        };

        builder.HasData(users);
    }
}