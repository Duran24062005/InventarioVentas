using FluentValidation;
using InventarioVentas.API.Modules.Productos.DTOs;

namespace InventarioVentas.API.Modules.Productos.Validators;

public class CrearProductoValidator : AbstractValidator<CrearProductoDto>
{
    public CrearProductoValidator()
    {
        // Valida que el nombre no sea nulo ni esté vacío
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio");

        // Valida que el código no sea vacío.
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El código del producto es obligatorio.");
        
        // Regla de negocio: El precio debe ser strictly mayor a 0 (HU02).
        RuleFor(x => x.Precio)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.");

        // Regla de negocio: El stock no puede ser negativo (HU02).
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");

        // Valida que se asigne una categora válida.
        RuleFor(x => x.CategoriaId)
            .GreaterThan(0).WithMessage("Debe especificar una categoría válida");
    }

}