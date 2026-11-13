using FluentValidation;
using Mantenimiento.Core.Application.DTOs.CuentaInstitucion;

namespace Mantenimiento.Core.Application.Validators.CuentaInstitucion
{
    public class CrearCuentaInstitucionValidator : AbstractValidator<CrearCuentaInstitucionDto>
    {
        public CrearCuentaInstitucionValidator()
        {
            RuleFor(x => x.CtaCtaBanco)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El número de cuenta es requerido.")
                .Matches(@"^\d+$")
                .WithMessage("La cuenta de banco debe contener únicamente números.");
        }
    }
}
