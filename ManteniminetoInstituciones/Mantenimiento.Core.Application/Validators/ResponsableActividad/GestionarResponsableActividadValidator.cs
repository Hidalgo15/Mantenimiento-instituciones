using FluentValidation;
using Mantenimiento.Core.Application.DTOs.ResponsableActividad;

namespace Mantenimiento.Core.Application.Validators.ResponsableActividad
{
    public class GestionarResponsableActividadValidator : AbstractValidator<GestionarResponsableActividadDto>
    {
        public GestionarResponsableActividadValidator()
        {
            RuleFor(x => x.CodigoDespa)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El código de despacho es obligatorio.");
        }
    }
}
