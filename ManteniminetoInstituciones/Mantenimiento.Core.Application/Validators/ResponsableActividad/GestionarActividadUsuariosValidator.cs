using FluentValidation;
using Mantenimiento.Core.Application.DTOs.ResponsableActividad;

namespace Mantenimiento.Core.Application.Validators.ResponsableActividad
{
    public class GestionarActividadUsuariosValidator : AbstractValidator<GestionarActividadUsuariosDto>
    {
        public GestionarActividadUsuariosValidator()
        {
            RuleFor(x => x.CodigoActividad)
                .GreaterThan(0)
                .WithMessage("El código de actividad es obligatorio.");
        }
    }
}
