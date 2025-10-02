using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(si => si.Id);

            builder.Property(si => si.Id)
                .ValueGeneratedOnAdd();

            builder.Property(si => si.SaleId)
                .IsRequired();

            builder.Property(si => si.Quantity)
                .IsRequired();

            builder.Property(si => si.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(si => si.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(si => si.DiscountPercent)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0);

            builder.Property(si => si.IsCancelled)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(si => si.CreatedAt)
                .IsRequired();

            builder.Property(si => si.UpdatedAt);

            // Relacionamentos
            builder.HasOne(si => si.Product)
                .WithMany()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(si => si.SaleId);
            builder.HasIndex("ProductId");
        }
    }
}