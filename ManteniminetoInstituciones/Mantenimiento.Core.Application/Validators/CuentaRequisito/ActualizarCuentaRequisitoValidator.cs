using FluentValidation;
using Mantenimiento.Core.Application.DTOs.CuentaRequisito;

namespace Mantenimiento.Core.Application.Validators.CuentaRequisito
{
    public class ActualizarCuentaRequisitoValidator : AbstractValidator<ActualizarCuentaRequisitoDto>
    {
        public ActualizarCuentaRequisitoValidator()
        {
            RuleFor(x => x.CuentaRequisitosID)
                .GreaterThan(0)
                .WithMessage("El ID de la relación es inválido.");

            RuleFor(x => x.IdCuenta)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar una cuenta válida.");

            RuleFor(x => x.IdRequisito)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un requisito válido.");
        }
    }
}
