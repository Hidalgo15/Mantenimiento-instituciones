namespace Mantenimiento.Core.Application.DTOs.Requisito
{
    public record RequisitoDto
    {
        public int RequisitosID { get; set; }
        public int IdRequisitos { get; set; }
        public string Requisitos { get; set; } = string.Empty;
    }
}
