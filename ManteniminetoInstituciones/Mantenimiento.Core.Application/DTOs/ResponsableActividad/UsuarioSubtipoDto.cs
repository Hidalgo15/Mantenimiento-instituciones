namespace Mantenimiento.Core.Application.DTOs.ResponsableActividad
{
    public record UsuarioSubtipoDto
    {
        public string CodigoDespa { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Institucion { get; set; }
        public string? DescArea { get; set; }
        public string? Cargo { get; set; }
        public string? Email { get; set; }
    }
}
