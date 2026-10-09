namespace Mantenimiento.Core.Application.DTOs.Institucion
{
    public record InstitucionSeleccionDto
    {
        public int IdInstitucion { get; set; }
        public string Estructura { get; set; } = string.Empty;
        public string UnidadEjecutora { get; set; } = string.Empty;
    }
}
