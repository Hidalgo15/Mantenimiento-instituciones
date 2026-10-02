namespace Mantenimiento.Core.Domain.Entities
{
    public class AnalistaSupervisorConsulta
    {
        public string Despacho_Analista { get; set; } = string.Empty;
        public string Nombre_Analista { get; set; } = string.Empty;
        public string Estado_Analista { get; set; } = string.Empty;
        public string? Despacho_Supervisor { get; set; }
        public string? Nombre_Supervisor { get; set; }
    }
}
