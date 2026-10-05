namespace Mantenimiento.Core.Application.DTOs.AnalistaSupervisor
{
    public record AnalistaSupervisorConsultaDto
    {
        public string DespachoAnalista { get; set; } = string.Empty;
        public string NombreAnalista { get; set; } = string.Empty;
        public string EstadoAnalista { get; set; } = string.Empty; // 'ASIGNADO' o 'PENDIENTE DE ASIGNACION'
        public string? DespachoSupervisor { get; set; }
        public string? NombreSupervisor { get; set; }
    }
}
