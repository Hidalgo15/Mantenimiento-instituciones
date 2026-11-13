using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Requisito;

namespace Mantenimiento.Core.Application.Validators.Requisito
{
    public class CrearRequisitoValidator : AbstractValidator<CrearRequisitoDto>
    {
        public CrearRequisitoValidator()
        {
            RuleFor(x => x.RequisitoDescripcion)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("La descripción del requisito es obligatoria.");
        }
    }
}
