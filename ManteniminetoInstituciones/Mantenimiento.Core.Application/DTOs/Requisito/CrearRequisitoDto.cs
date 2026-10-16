using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.Requisito
{
    public record CrearRequisitoDto : AuditableDto
    {
        public string RequisitoDescripcion { get; set; } = string.Empty;
    }
}
