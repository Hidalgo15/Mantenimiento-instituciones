namespace Mantenimiento.Core.Application.DTOs.Capitulo
{
    public record CapituloDto
    {
        public int Id { get; set; }
        public string CodigoCapitulo { get; set; } = string.Empty;
        public string NombreCapitulo { get; set; } = string.Empty;
    }
}
