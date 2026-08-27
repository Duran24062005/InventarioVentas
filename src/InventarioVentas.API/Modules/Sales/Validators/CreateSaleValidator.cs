using FluentValidation;
using InventarioVentas.API.Modules.Sales.DTOs;

namespace InventarioVentas.API.Modules.Sales.Validators;

public class CreateSaleValidator : AbstractValidator <CreateSaleDto>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Debe especificar un cliente valido");

        RuleFor(x => x.Details)
            .NotEmpty().WithMessage("El detalle es obligatorio");
        
        RuleForEach(x => x.Details)
            .SetValidator(new CreateSaleDetailValidator());

        RuleFor(x => x.Details)
            .Must(details => 
                details.Select(detail => detail.ProductId)
                    .Distinct()
                    .Count() == details.Count)
            .WithMessage("Nose de puede repetir un producto de la misma venta");


    }
}

public class CreateSaleDetailValidator : AbstractValidator <CreateSaleDetailDto>
{
    
    // Revisa la validacion de cada produccto y cantidad del SaleDetail
    public CreateSaleDetailValidator ()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Debe especificar un producto valido");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("El cantidad debe ser mayor a cero");
    }

}