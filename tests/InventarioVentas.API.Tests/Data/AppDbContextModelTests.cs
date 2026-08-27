using InventarioVentas.API.Modules.Categories.Models;
using InventarioVentas.API.Modules.Products.Models;
using InventarioVentas.API.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentas.API.Tests.Data;

public class AppDbContextModelTests : SqliteTestBase
{
    [Fact]
    public void Configures_one_restrictive_category_product_relationship()
    {
        var categoryEntity = Context.Model.FindEntityType(typeof(Category));
        var productEntity = Context.Model.FindEntityType(typeof(Product));

        Assert.NotNull(categoryEntity);
        Assert.NotNull(productEntity);
        Assert.DoesNotContain(categoryEntity!.GetProperties(), property =>
            property.Name == "ProductId");

        var foreignKey = Assert.Single(productEntity!.GetForeignKeys(), foreignKey =>
            foreignKey.Properties.Single().Name == nameof(Product.CategoryId));

        Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior);
        Assert.True(productEntity.FindIndex(new[] { productEntity.FindProperty(nameof(Product.Code))! })!.IsUnique);
    }
}
