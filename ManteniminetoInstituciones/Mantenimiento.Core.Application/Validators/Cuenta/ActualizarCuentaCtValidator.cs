using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO;

namespace Mantenimiento.Core.Application.Validators.Cuenta
{
    public class ActualizarCuentaCtValidator : AbstractValidator<ActualizarCuentaCtDto>
    {
        public ActualizarCuentaCtValidator()
        {
            RuleFor(x => x.CuentaID).GreaterThan(0).WithMessage("El ID de la cuenta no es válido.");
            RuleFor(x => x.CodigoCuenta)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El código de cuenta no puede estar vacío.");
            RuleFor(x => x.DescripcionCuenta)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("La descripción de la cuenta no puede estar vacía.");
        }
    }
}
