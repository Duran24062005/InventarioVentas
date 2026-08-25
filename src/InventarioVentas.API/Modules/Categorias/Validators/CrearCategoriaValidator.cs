using FluentValidation;
using System.Runtime.InteropServices;
using InventarioVentas.API.Modules.Categorias.DTOs;

namespace InventarioVentas.API.Modules.Categorias.Validators
{
    public class CrearCategoriaValidator
        : AbstractValidator<CrearCategoriaDto>
    {
        public CrearCategoriaValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Descripcion)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.FechaCreacion)
                .NotEmpty();

            RuleFor(x => x.Estado)
                .NotEmpty();
        }
    }
}
