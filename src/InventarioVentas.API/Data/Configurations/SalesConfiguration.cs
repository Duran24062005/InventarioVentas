using InventarioVentas.API.Modules.Sales.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioVentas.API.Data.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(sale => sale.Id);

        builder.Property(sale => sale.SaleDate)
            .IsRequired();

        builder.Property(sale => sale.Total)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(sale => sale.Customer)
            .WithMany()
            .HasForeignKey(sale => sale.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sale => sale.Details)
            .WithOne(detail => detail.Sale)
            .HasForeignKey(detail => detail.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SaleDetailsConfiguration : IEntityTypeConfiguration<SaleDetails>
{
    public void Configure(EntityTypeBuilder<SaleDetails> builder)
    {
        builder.ToTable("SaleDetails");
        builder.HasKey(detail => detail.Id);

        builder.Property(detail => detail.Quantity)
            .IsRequired();

        builder.Property(detail => detail.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(detail => detail.Subtotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(detail => detail.Product)
            .WithMany()
            .HasForeignKey(detail => detail.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
