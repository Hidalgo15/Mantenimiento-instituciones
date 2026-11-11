using FluentValidation;
using Mantenimiento.Core.Application.DTOs.ProveedoresInhabilitados;

namespace Mantenimiento.Core.Application.Validators.ProveedoresInhabilitados
{
    public class ActualizarProveedorInhabilitadoValidator : AbstractValidator<ActualizarProveedorInhabilitadoDto>
    {
        public ActualizarProveedorInhabilitadoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El identificador del proveedor es inválido.");

            RuleFor(x => x.RazonSocial)
                .NotEmpty()
                .WithMessage("La razón social o nombre del proveedor es requerido.");

            RuleFor(x => x)
                .Must(dto => !string.IsNullOrWhiteSpace(dto.Rpe) || !string.IsNullOrWhiteSpace(dto.NumeroDocumento))
                .WithMessage("Debe proporcionar el RPE o el número de documento del proveedor.");
        }
    }
}
