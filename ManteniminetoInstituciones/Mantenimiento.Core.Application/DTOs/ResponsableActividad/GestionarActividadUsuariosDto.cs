using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record GestionarActividadUsuariosDto : AuditableDto
    {
        public int CodigoActividad { get; set; }

        public int CodigoTre { get; set; }

        public string? ListaCodigosDespa { get; set; }
    }
}
