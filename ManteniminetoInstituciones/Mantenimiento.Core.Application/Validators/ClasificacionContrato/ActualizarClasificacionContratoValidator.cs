using FluentValidation;
using Mantenimiento.Core.Application.DTOs.ClasificacionContrato;

namespace Mantenimiento.Core.Application.Validators.ClasificacionContrato
{
    public class ActualizarClasificacionContratoValidator : AbstractValidator<ActualizarClasificacionContratoDto>
    {
        public ActualizarClasificacionContratoValidator()
        {
            RuleFor(x => x.CodigoClasificacion).GreaterThan(0)
                .WithMessage("El código de clasificación es inválido.");
        }
    }
}
