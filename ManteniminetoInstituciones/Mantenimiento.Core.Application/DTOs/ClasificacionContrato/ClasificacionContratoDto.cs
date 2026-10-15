namespace Mantenimiento.Core.Application.DTOs.ClasificacionContrato
{
    public record ClasificacionContratoDto
    {
        public int CodigoClasificacion { get; set; }
        public string? ContratoTipo { get; set; }
        public string? TipoOperacion { get; set; }
        public string? ContratoProcesoSeleccion { get; set; }
    }
}
