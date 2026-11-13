using FluentValidation;
using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;

namespace Mantenimiento.Core.Application.Validators.CuentaInstitucion
{
    public class ActualizarCuentaInstitucionValidator : AbstractValidator<ActualizarCuentaInstitucionDto>
    {
        public ActualizarCuentaInstitucionValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El identificador de la cuenta es inválido.");
            RuleFor(x => x.CtaCtaBanco)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El número de cuenta es requerido.")
                .Matches(@"^\d+$")
                .WithMessage("La cuenta de banco debe contener únicamente números.");
        }
    }
}
