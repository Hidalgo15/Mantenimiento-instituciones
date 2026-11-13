using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Cuenta.DboCuentasDTO;

namespace Mantenimiento.Core.Application.Validators.Cuenta
{
    public class CrearCuentaValidator : AbstractValidator<CrearCuentaDto>
    {
        public CrearCuentaValidator()
        {
            RuleFor(x => x.IdCuentaBase).GreaterThan(0)
                .WithMessage("Debe seleccionar una cuenta base válida.");
            RuleFor(x => x.TipoTramite).GreaterThan(0)
                .WithMessage("Debe seleccionar un tipo de trámite de pago válido.");
        }
    }
}
