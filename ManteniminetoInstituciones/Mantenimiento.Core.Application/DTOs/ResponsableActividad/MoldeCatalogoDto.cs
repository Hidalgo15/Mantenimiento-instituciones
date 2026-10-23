namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record MoldeCatalogoDto
    {
        public int CodigoTre { get; set; }
        public string? ArbolAsociado { get; set; }
        public string? NombreTre { get; set; }
    }
}
