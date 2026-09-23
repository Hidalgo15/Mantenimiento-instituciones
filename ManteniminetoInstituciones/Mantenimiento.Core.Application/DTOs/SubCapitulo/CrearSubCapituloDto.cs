namespace Mantenimiento.Core.Application.DTOs.SubCapitulo
{
    public class CrearSubCapituloDto
    {
       public int IdCapitulo { get; set; }
       public int CodigoSubcapitulo { get; set; }
       public string subcapitulo { get; set; } = string.Empty;
    }
}
