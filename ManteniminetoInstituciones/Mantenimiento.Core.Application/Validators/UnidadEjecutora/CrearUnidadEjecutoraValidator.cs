using FluentValidation;
using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;

namespace Mantenimiento.Core.Application.Validators.UnidadEjecutora
{
    public class CrearUnidadEjecutoraValidator : AbstractValidator<CrearUnidadEjecutoraDto>
    {
        public CrearUnidadEjecutoraValidator()
        {
            RuleFor(x => x.IdDaf)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar una DAF válida.");

            RuleFor(x => x.IdCategoria)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar una categoría válida.");

            RuleFor(x => x.CodigoUnidadEjecutora)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El código de unidad ejecutora es obligatorio.")
                .Must(value => value.Trim().Length <= 10)
                .WithMessage("El código de unidad ejecutora no puede tener más de 10 caracteres.");

            RuleFor(x => x.NombreUnidadEjecutora)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("El nombre de la unidad ejecutora es obligatorio.")
                .Must(value => value.Trim().Length <= 250)
                .WithMessage("El nombre de la unidad ejecutora no puede tener más de 250 caracteres.");

            RuleFor(x => x.Rnc)
                .Must(value => string.IsNullOrWhiteSpace(value) || value.Trim().Length <= 20)
                .WithMessage("El RNC no puede superar los 20 caracteres.");
        }
    }
}
