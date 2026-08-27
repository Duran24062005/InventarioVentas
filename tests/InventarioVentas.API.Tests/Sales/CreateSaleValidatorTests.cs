using InventarioVentas.API.Modules.Sales.DTOs;
using InventarioVentas.API.Modules.Sales.Validators;

namespace InventarioVentas.API.Tests.Sales;

public class CreateSaleValidatorTests
{
    private readonly CreateSaleValidator _validator = new();

    [Fact]
    public void Rejects_a_sale_without_details()
    {
        var result = _validator.Validate(new CreateSaleDto());

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error =>
            error.PropertyName == nameof(CreateSaleDto.Details));
    }

    [Fact]
    public void Rejects_repeated_products_in_the_same_sale()
    {
        var productId = Guid.NewGuid();
        var sale = new CreateSaleDto
        {
            CustomerId = Guid.NewGuid(),
            Details =
            [
                new CreateSaleDetailDto { ProductId = productId, Quantity = 1 },
                new CreateSaleDetailDto { ProductId = productId, Quantity = 2 }
            ]
        };

        var result = _validator.Validate(sale);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error =>
            error.ErrorMessage.Contains("repetir", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Rejects_a_non_positive_quantity()
    {
        var sale = new CreateSaleDto
        {
            CustomerId = Guid.NewGuid(),
            Details =
            [
                new CreateSaleDetailDto { ProductId = Guid.NewGuid(), Quantity = 0 }
            ]
        };

        var result = _validator.Validate(sale);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error =>
            error.PropertyName == "Details[0].Quantity");
    }
}
