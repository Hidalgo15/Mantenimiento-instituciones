using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Daf;

namespace Mantenimiento.Core.Application.Validators.Daf
{
    public class ActualizarDafValidator : AbstractValidator<ActualizarDafDTO>
    {
        public ActualizarDafValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El identificador del daf es inválido.");
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
