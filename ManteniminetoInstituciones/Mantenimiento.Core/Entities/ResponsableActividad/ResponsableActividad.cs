namespace Mantenimiento.Core.Domain.Entities.ResponsableActividad
{
    public class ResponsableActividad
    {
        public string CodigoDespa { get; set; } = string.Empty;
        public int CodigoActividad { get; set; }
        public string? DescripcionActividad { get; set; }
    }
}
