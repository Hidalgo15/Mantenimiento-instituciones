using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Requisito;

namespace Mantenimiento.Core.Application.Validators.Requisito
{
    public class ActualizarRequisitoValidator : AbstractValidator<ActualizarRequisitoDto>
    {
        public ActualizarRequisitoValidator()
        {
            RuleFor(x => x.RequisitosID).GreaterThan(0).WithMessage("El ID del requisito es inválido.");
            RuleFor(x => x.RequisitoDescripcion)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("La descripción del requisito no puede estar vacía.");
        }
    }
}
