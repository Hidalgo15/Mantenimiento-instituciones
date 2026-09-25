
namespace Mantenimiento.Core.Application.DTOs.SubCapitulo
{
    public class ActualizarSubCapituloDto
    {

        public int Id { get; set; }
        public int IdCapitulo { get; set; }
        public string? CodigoCapitulo { get; set; }
        public string? Capitulo { get; set; }
        public string? CodigoSubCapitulo { get; set; } // Asegúrate de que sea string si en SQL es VARCHAR/CHAR
        public string? subcapitulo { get; set; }
    }
}
