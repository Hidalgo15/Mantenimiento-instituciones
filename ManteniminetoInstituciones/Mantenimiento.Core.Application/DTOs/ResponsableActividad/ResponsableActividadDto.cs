namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record ResponsableActividadDto
    {
        public string CodigoDespa { get; set; } = string.Empty;
        public int CodigoActividad { get; set; }
        public string? DescripcionActividad { get; set; }
    }
}
