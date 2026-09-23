
namespace Mantenimiento.Core.Application.DTOs.SubCapitulo
{
    public class SubCapituloDto
    {
        public int Id { get; set; }
        public int IdCapitulo { get; set; }
        public int CodigoSubcapitulo { get; set; }
        public string subcapitulo { get; set; } = string.Empty;
    }
}
