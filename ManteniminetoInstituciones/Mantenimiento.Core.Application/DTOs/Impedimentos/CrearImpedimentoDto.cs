namespace Mantenimiento.Core.Application.DTOs.Impedimentos
{
    public record CrearImpedimentoDto
    {
        public string? Nombres { get; set; }
        public string? CedulaRnc { get; set; }
        public string? UsuarioAgrego { get; set; }
        public string? Motivo { get; set; }
        public bool Estado { get; set; } = true;
    }
}
