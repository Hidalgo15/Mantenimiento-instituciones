using FluentValidation;
using Mantenimiento.Core.Application.DTOs.RelacionContratoRequisito;

namespace Mantenimiento.Core.Application.Validators.RelacionContratoRequisito
{
    public class CrearRelacionContratoRequisitoValidator : AbstractValidator<CrearRelacionContratoRequisitoDto>
    {
        public CrearRelacionContratoRequisitoValidator()
        {
            RuleFor(x => x.CodigoClasificacion).GreaterThan(0)
                .WithMessage("Debe seleccionar una clasificación de contrato válida.");
            RuleFor(x => x.CodigoRequisito).GreaterThan(0)
                .WithMessage("Debe seleccionar un requisito válido.");
        }
    }
}
