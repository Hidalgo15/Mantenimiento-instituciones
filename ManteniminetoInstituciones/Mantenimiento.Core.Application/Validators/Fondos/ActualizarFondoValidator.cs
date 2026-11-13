using FluentValidation;
using Mantenimiento.Core.Application.DTOs.Fondos;

namespace Mantenimiento.Core.Application.Validators.Fondos
{
    public class ActualizarFondoValidator : AbstractValidator<FondoDto>
    {
        public ActualizarFondoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El identificador del fondo es inválido.");
            RuleFor(x => x.IdInstitucion).GreaterThan(0)
                .WithMessage("Debe seleccionar una institución válida.");
            RuleFor(x => x.Fondo)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El campo fondo es requerido.")
                .Must(value => !int.TryParse(value, out var amount) || amount > 0)
                .WithMessage("El campo fondo debe ser un número positivo.");
        }
    }
}
