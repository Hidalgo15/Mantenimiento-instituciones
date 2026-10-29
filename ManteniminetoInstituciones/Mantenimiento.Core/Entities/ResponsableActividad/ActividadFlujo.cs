namespace Mantenimiento.Core.Domain.Entities.ResponsableActividad
{
    public class ActividadFlujo
    {
        public int CodigoActividad { get; set; }
        public int CodigoTipoTre { get; set; }
        public string DescripcionActividad { get; set; } = string.Empty;
        public string? DescripcionSubtipo { get; set; }
        public string? FlujoCorrespondiente { get; set; }
    }
}
