namespace Mantenimiento.Core.Application.DTOs.CuentaInstitucion
{
    public record InstitucionSeleccionDto
    {
        public string Estructura { get; set; }       // ins_codigo
        public string UnidadEjecutora { get; set; }  // Institucion
    }
}
