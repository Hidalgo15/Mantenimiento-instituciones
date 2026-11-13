namespace Mantenimiento.Core.Application.DTOs.ProveedoresInhabilitados
{
    public record ProveedorInhabilitadoDto
    {
        public int Id { get; set; }
        public string? Rpe { get; set; }
        public string? RazonSocial { get; set; }
        public string? Genero { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? TipoDocumento { get; set; }
        public string? TipoPersona { get; set; }
        public string? FechaRegistro { get; set; }
        public string? FechaUltimaActualizacion { get; set; }
        public string? EstadoRpe { get; set; }
        public string? MotivoInhabilitacion { get; set; }
    }
}
