using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Daf;

namespace Mantenimiento.Core.Application.Validators.Daf
{
    public class CrearDafValidator : AbstractValidator<CrearDafDTO>
    {
        public CrearDafValidator()
        {
            RuleFor(x => x.CodigoDaf)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El código del daf es requerido.")
                .MaximumLength(10)
                .WithMessage("El código del daf no puede exceder 10 dígitos.");

            RuleFor(x => x.Daf)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El nombre del daf es requerido.");
        }
    }
}
