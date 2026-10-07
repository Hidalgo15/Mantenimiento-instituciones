namespace Mantenimiento.Core.Application.DTOs.UnidadEjecutora
{
    public record UnidadEjecutoraDto
    {
        public int Id { get; set; }
        public int? IdPadre { get; set; }
        public int IdDaf { get; set; }
        public string CodigoDaf { get; set; } = string.Empty;
        public string NombreDaf { get; set; } = string.Empty;
        public string CodigoUnidadEjecutora { get; set; } = string.Empty;
        public string NombreUnidadEjecutora { get; set; } = string.Empty;
        public string? Rnc { get; set; }
        public bool Estado { get; set; }
        public int? PortalCompra { get; set; }

        public int? IdCategoria { get; set; }
        public string? Estructura { get; set; }
        public string? DescripcionCategoria { get; set; }
    }
}
