using FluentValidation;
using InventarioVentas.API.Modules.Customers.DTOs;


namespace InventarioVentas.API.Modules.Customers.Validators
{
    public class CreateCustomerValidator
        : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.NombreCompleto)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Documento)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Telefono)
                .NotEmpty()
                .MaximumLength(100);






        }
    }
}
