using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Cuenta.CtCuentasDTO;

namespace Mantenimiento.Core.Application.Validators.Cuenta
{
    public class CrearCuentaCtValidator : AbstractValidator<CrearCuentaCtDto>
    {
        public CrearCuentaCtValidator()
        {
            RuleFor(x => x.CodigoCuenta)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El código de cuenta es obligatorio.");
            RuleFor(x => x.DescripcionCuenta)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("La descripción de la cuenta es obligatoria.");
        }
    }
}
