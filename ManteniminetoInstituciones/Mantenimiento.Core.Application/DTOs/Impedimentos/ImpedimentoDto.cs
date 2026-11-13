namespace Mantenimiento.Core.Application.DTOs.Impedimentos
{
    public record ImpedimentoDto
    {
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? CedulaRnc { get; set; }
        public string? UsuarioAgrego { get; set; }
        public string? Motivo { get; set; }
        public DateTime? FechaAgrego { get; set; }
        public bool? Estado { get; set; }
    }
}
