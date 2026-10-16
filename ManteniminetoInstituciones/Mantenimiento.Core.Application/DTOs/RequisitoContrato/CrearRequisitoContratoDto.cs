using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.RequisitoContrato
{
    public record CrearRequisitoContratoDto : AuditableDto
    {
        public string? DescripcionRequisito { get; set; }
        public int? CodigoDocumentoTre { get; set; }
    }
}
