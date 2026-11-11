using Mantenimiento.Core.Application.DTOs.Auditoria;

namespace Mantenimiento.Core.Application.DTOs.ProveedoresInhabilitados
{
    public record ActualizarProveedorInhabilitadoDto : AuditableDto
    {
        public int Id { get; set; }
        public string? Rpe { get; set; }
        public string? RazonSocial { get; set; }
        public string? Genero { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? TipoDocumento { get; set; }
        public string? TipoPersona { get; set; }
        public string? EstadoRpe { get; set; }
        public string? MotivoInhabilitacion { get; set; }
    }
}
