using FluentValidation;
using Mantenimiento.Core.Application.DTOs.AnalistaSupervisor;

namespace Mantenimiento.Core.Application.Validators.AnalistaSupervisor
{
    public class AsignarSupervisorValidator : AbstractValidator<AsignarSupervisorDto>
    {
        public AsignarSupervisorValidator()
        {
            RuleFor(x => x.DespachoAnalista)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El despacho del analista es obligatorio.")
                .MaximumLength(20)
                .WithMessage("El despacho no debe superar los 20 caracteres.");

            RuleFor(x => x.DespachoSupervisor)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("Debe seleccionar un supervisor.")
                .MaximumLength(20)
                .WithMessage("El despacho del supervisor no debe superar los 20 caracteres.");
        }
    }
}
