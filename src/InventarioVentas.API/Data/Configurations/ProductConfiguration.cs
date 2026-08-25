namespace InventarioVentas.API.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventarioVentas.API.Modules.Productos.Models;


public class ProductConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos");


        builder.HasKey(p => p.Id);


        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(150);


        builder.Property(p => p.Codigo)
            .IsRequired()
            .HasMaxLength(50);


        builder.HasIndex(p => p.Codigo)
            .IsUnique();


        builder.Property(p => p.Precio)
            .HasColumnType("decimal(18,2)");


        builder.Property(p => p.Stock)
            .IsRequired();


        builder.Property(p => p.Estado)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(p => p.FechaCreacion)
            .IsRequired();
        

        builder.HasOne(p => p.Categoria)
            .WithMany()
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}