using FluentValidation;
using Mantenimiento.Core.Application.DTOs.CuentaRequisito;

namespace Mantenimiento.Core.Application.Validators.CuentaRequisito
{
    public class CrearCuentaRequisitoValidator : AbstractValidator<CrearCuentaRequisitoDto>
    {
        public CrearCuentaRequisitoValidator()
        {
            RuleFor(x => x.IdCuenta)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar una cuenta válida.");

            RuleFor(x => x.IdRequisito)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un requisito válido.");
        }
    }
}
