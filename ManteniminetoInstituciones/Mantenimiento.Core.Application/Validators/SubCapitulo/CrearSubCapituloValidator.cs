using FluentValidation;
using Mantenimiento.Core.Application.DTOs.SubCapitulo;

namespace Mantenimiento.Core.Application.Validators.SubCapitulo
{
    public class CrearSubCapituloValidator : AbstractValidator<CrearSubCapituloDto>
    {
        public CrearSubCapituloValidator()
        {
            RuleFor(x => x.CodigoSubCapitulo)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El código del subcapítulo es requerido.")
                .MaximumLength(10)
                .WithMessage("El código del subcapítulo no puede exceder 10 dígitos.");

            RuleFor(x => x.subcapitulo)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El nombre del subcapítulo es requerido.");
        }
    }
}
