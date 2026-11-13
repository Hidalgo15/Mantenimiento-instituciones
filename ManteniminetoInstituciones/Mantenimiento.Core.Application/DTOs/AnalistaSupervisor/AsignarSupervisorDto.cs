using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.AnalistaSupervisor
{
    public record AsignarSupervisorDto : AuditableDto
    {
        public string DespachoAnalista { get; set; } = string.Empty;

        public string DespachoSupervisor { get; set; } = string.Empty;
    }
}
