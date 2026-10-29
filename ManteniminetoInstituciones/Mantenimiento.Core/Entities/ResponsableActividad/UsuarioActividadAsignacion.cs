namespace Mantenimiento.Core.Domain.Entities.ResponsableActividad
{
    public class UsuarioActividadAsignacion
    {
        public string CodigoDespa { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Institucion { get; set; }
        public string? DescArea { get; set; }
        public string? Cargo { get; set; }
        public string? Email { get; set; }
        public bool EstaAsignado { get; set; }
    }
}
