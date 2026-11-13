using FluentValidation;
using Mantenimiento.Core.Application.DTOs.RelacionContratoRequisito;

namespace Mantenimiento.Core.Application.Validators.RelacionContratoRequisito
{
    public class ActualizarRelacionContratoRequisitoValidator : AbstractValidator<ActualizarRelacionContratoRequisitoDto>
    {
        public ActualizarRelacionContratoRequisitoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID de la relación es inválido.");
            RuleFor(x => x.CodigoClasificacion).GreaterThan(0)
                .WithMessage("Debe seleccionar una clasificación de contrato válida.");
            RuleFor(x => x.CodigoRequisito).GreaterThan(0)
                .WithMessage("Debe seleccionar un requisito válido.");
        }
    }
}
