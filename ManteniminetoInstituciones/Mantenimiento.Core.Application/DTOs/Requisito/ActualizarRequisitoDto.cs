namespace Mantenimiento.Core.Application.DTOs.Requisito
{
    public record ActualizarRequisitoDto
    {
        public int RequisitosID { get; set; }
        public string RequisitoDescripcion { get; set; } = string.Empty;
    }
}
