using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Capitulo;

namespace Mantenimiento.Core.Application.Validators.Capitulo
{
    public class ActualizarCapituloValidator : AbstractValidator<ActualizarCapituloDto>
    {
        public ActualizarCapituloValidator()
        {
            RuleFor(x => x.CodigoCapitulo)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El código del capítulo es requerido.")
                .MaximumLength(10)
                .WithMessage("El código del capítulo no puede exceder 10 caracteres.");

            RuleFor(x => x.NombreCapitulo)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El nombre del capítulo es requerido.");
        }
    }
}
