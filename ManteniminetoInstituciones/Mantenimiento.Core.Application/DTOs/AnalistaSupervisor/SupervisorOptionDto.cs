namespace Mantenimiento.Core.Application.DTOs.AnalistaSupervisor
{
    public record SupervisorOptionDto
    {
        public string DespachoSupervisor { get; set; } = string.Empty;
        public string NombreSupervisor { get; set; } = string.Empty;
    }
}
