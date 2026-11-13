using FluentValidation;
using Mantenimiento.Core.Application.DTOs.RequisitoContrato;

namespace Mantenimiento.Core.Application.Validators.RequisitoContrato
{
    public class CrearRequisitoContratoValidator : AbstractValidator<CrearRequisitoContratoDto>
    {
        public CrearRequisitoContratoValidator()
        {
            RuleFor(x => x.DescripcionRequisito)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("La descripción del requisito es requerida.")
                .Must(value => value!.Trim().Length <= 300)
                .WithMessage("La descripción del requisito no puede exceder los 300 caracteres.");
        }
    }
}
