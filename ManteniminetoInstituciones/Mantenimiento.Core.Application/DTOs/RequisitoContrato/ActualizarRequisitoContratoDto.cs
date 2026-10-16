using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.RequisitoContrato
{
    public record ActualizarRequisitoContratoDto : AuditableDto
    {
        public int CodigoRequisito { get; set; }
        public string? DescripcionRequisito { get; set; }
        public int? CodigoDocumentoTre { get; set; }
    }
}
