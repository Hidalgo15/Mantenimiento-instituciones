using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.Requisito
{
    public record ActualizarRequisitoDto : AuditableDto
    {
        public int RequisitosID { get; set; }
        public string RequisitoDescripcion { get; set; } = string.Empty;
    }
}
