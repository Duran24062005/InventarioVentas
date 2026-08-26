using FluentValidation;
using InventarioVentas.API.Modules.Products.DTOs;

namespace InventarioVentas.API.Modules.Products.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        // The product name is required.
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio");

        // The product code is required.
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código del producto es obligatorio.");

        // Business rule: price must be greater than zero (HU02).
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.");

        // Business rule: stock cannot be negative (HU02).
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");

        // A valid category must be assigned.
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Debe especificar una categoría válida");
    }



}
