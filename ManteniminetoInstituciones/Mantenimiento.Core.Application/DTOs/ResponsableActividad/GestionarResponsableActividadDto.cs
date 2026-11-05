using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record GestionarResponsableActividadDto : AuditableDto
    {
        public string CodigoDespa { get; set; } = string.Empty;

        public string? ListaCodigosActividades { get; set; }
    }
}
