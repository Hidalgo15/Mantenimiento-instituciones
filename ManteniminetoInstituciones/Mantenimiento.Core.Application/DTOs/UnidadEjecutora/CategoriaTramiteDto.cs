namespace Mantenimiento.Core.Application.DTOs.UnidadEjecutora
{
    public record CategoriaTramiteDto
    {
        public int CodigoCategoria { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}