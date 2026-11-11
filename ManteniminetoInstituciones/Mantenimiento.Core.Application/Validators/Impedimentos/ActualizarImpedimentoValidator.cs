using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Impedimentos;

namespace Mantenimiento.Core.Application.Validators.Impedimentos
{
    public class ActualizarImpedimentoValidator : AbstractValidator<ActualizarImpedimentoDto>
    {
        public ActualizarImpedimentoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El identificador del impedimento es inválido.");

            RuleFor(x => x)
                .Must(dto => !string.IsNullOrWhiteSpace(dto.Nombres) || !string.IsNullOrWhiteSpace(dto.CedulaRnc))
                .WithMessage("Debe proporcionar el nombre o la cédula/RNC del impedido.");

            RuleFor(x => x.Nombres)
                .MaximumLength(255)
                .WithMessage("El nombre no puede exceder 255 caracteres.");

            RuleFor(x => x.CedulaRnc)
                .MaximumLength(15)
                .WithMessage("La cédula/RNC no puede exceder 15 caracteres.");

            RuleFor(x => x.Motivo)
                .MaximumLength(500)
                .WithMessage("El motivo no puede exceder 500 caracteres.");
        }
    }
}
