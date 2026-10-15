namespace Mantenimiento.Core.Application.DTOs.ClasificacionContrato
{
    public record ProcesoSeleccionCatalogoDto
    {
        public int Id { get; set; }
        public string Proceso { get; set; } = string.Empty;
    }
}
