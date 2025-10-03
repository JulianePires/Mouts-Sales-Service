using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.SaleNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SaleDate)
                .IsRequired();

            // Foreign Keys
            builder.Property(s => s.CustomerId)
                .IsRequired();

            builder.Property(s => s.BranchId)
                .IsRequired();

            builder.Property(s => s.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasDefaultValue(SaleStatus.Draft);

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.Property(s => s.UpdatedAt);

            // Relacionamentos
            builder.HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Branch)
                .WithMany()
                .HasForeignKey(s => s.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com SaleItems
            builder.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices para performance
            builder.HasIndex(s => s.SaleNumber)
                .IsUnique();

            builder.HasIndex(s => s.CustomerId);
            builder.HasIndex(s => s.BranchId);
            builder.HasIndex(s => s.SaleDate);

            // Seed data
            SaleSeed.Configure(builder);
        }
    }
}