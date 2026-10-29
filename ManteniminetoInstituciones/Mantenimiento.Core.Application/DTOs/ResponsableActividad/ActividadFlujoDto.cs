namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record ActividadFlujoDto
    {
        public int CodigoActividad { get; set; }
        public int CodigoTipoTre { get; set; }
        public string DescripcionActividad { get; set; } = string.Empty;
        public string? DescripcionSubtipo { get; set; }
        public string? FlujoCorrespondiente { get; set; }
    }
}
