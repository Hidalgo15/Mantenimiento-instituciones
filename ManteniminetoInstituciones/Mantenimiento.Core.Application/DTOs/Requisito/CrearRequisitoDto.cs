namespace Mantenimiento.Core.Application.DTOs.Requisito
{
    public record CrearRequisitoDto
    {
        public string RequisitoDescripcion { get; set; } = string.Empty;
    }
}
