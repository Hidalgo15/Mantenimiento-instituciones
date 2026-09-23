namespace Mantenimiento.Core.Application.DTOs.Capitulo
{
    public class ActualizarCapituloDto
    {
        public int Id { get; set; }
        public string CodigoCapitulo { get; set; } = string.Empty;
        public string NombreCapitulo { get; set; } = string.Empty;
    }
}
