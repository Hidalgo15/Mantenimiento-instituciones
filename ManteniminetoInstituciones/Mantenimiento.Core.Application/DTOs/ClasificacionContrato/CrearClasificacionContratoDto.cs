namespace Mantenimiento.Core.Application.DTOs.ClasificacionContrato
{
    public record CrearClasificacionContratoDto
    {
        public string? ContratoTipo { get; set; }
        public string? TipoOperacion { get; set; }
        public string? ContratoProcesoSeleccion { get; set; }
    }
}
