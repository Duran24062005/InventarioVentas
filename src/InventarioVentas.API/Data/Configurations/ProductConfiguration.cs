namespace InventarioVentas.API.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventarioVentas.API.Modules.Products.Models;


public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");


        builder.HasKey(p => p.Id);


        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);


        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

// Índice único obligatorio para el código de producto (PRD-003)

        builder.HasIndex(p => p.Code)
            .IsUnique();


        builder.Property(p => p.Price)
            .HasColumnType("numeric(18,2)")
            .IsRequired();


        builder.Property(p => p.Stock)
            .IsRequired();


        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(p => p.CreatedAt)
            .IsRequired();
        

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
