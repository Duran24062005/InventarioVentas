using FluentValidation;
using InventarioVentas.API.Modules.Categories.DTOs;

namespace InventarioVentas.API.Modules.Categories.Validators
{
    public class CreateCategoryValidator
        : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.CreatedAt)
                .NotEmpty();

            RuleFor(x => x.IsActive)
                .NotEmpty();
        }
    }
}
