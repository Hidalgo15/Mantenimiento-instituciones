using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Cuenta.DboCuentasDTO;

namespace Mantenimiento.Core.Application.Validators.Cuenta
{
    public class ActualizarCuentaValidator : AbstractValidator<ActualizarCuentaDto>
    {
        public ActualizarCuentaValidator()
        {
            RuleFor(x => x.IdCuenta).GreaterThan(0).WithMessage("El ID de la cuenta no es válido.");
            RuleFor(x => x.DescripcionCuenta)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("La descripción de la cuenta no puede estar vacía.");
            RuleFor(x => x.TipoTramite).GreaterThan(0)
                .WithMessage("Debe seleccionar un tipo de trámite válido.");
        }
    }
}
