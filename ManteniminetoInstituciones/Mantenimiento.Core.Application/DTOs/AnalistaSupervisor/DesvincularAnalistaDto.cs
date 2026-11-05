using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.AnalistaSupervisor
{
    public record DesvincularAnalistaDto : AuditableDto
    {
        public string DespachoAnalista { get; set; } = string.Empty;
    }
}
