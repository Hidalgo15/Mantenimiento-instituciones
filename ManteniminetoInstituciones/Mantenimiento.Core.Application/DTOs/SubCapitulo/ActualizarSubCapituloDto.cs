
namespace Mantenimiento.Core.Application.DTOs.SubCapitulo
{
    public class ActualizarSubCapituloDto
    {
       
        public int IdCapitulo { get; set; }
        public int CodigoSubcapitulo { get; set; }
        public string subcapitulo { get; set; } = string.Empty;
    }
}
