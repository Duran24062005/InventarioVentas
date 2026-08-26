using InventarioVentas.API.Modules.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentas.API.Data.Configurations;

public class ProductDbContext : DbContext
{
    public ProductDbContext(
        DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }


    public DbSet<Product> Products => Set<Product>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Price)
                .HasPrecision(18, 2);

            entity.Property(p => p.Stock)
                .IsRequired();

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(p => p.CategoryId)
                .IsRequired();

            entity.Property(p => p.CreatedAt)
                .IsRequired();
        });
    }
}














/// public class ProductConfiguration : IEntityTypeConfiguration<Product>
/// {
///     public void Configure(EntityTypeBuilder<Product> builder)
///     {
///         builder.ToTable("Products");
///
///
///         builder.HasKey(p => p.Id);
///
///
///         builder.Property(p => p.Name)
///             .IsRequired()
///             .HasMaxLength(150);
///
///
///         builder.Property(p => p.Code)
///             .IsRequired()
///             .HasMaxLength(50);
///
/// // Índice único obligatorio para el código de producto (PRD-003)
///
///         builder.HasIndex(p => p.Code)
///             .IsUnique();
///
///
///         builder.Property(p => p.Price)
///             .HasColumnType("numeric(18,2)")
///             .IsRequired();
///
///
///         builder.Property(p => p.Stock)
///             .IsRequired();
///
///
///         builder.Property(p => p.IsActive)
///             .IsRequired()
///             .HasDefaultValue(true);
///     
///         builder.Property(p => p.CreatedAt)
///             .IsRequired();
///     
///
///         builder.HasOne(p => p.Category)
///             .WithMany()
///             .HasForeignKey(p => p.CategoryId)
///             .OnDelete(DeleteBehavior.Restrict);
///     }
/// }
   ///