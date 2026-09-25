namespace Mantenimiento.Core.Application.DTOs.Daf
{
    public record DafDto
    {
        public int Id { get; set; }
        public int IdSubCapitulo { get; set; }
        public string CodigoSubCapitulo { get; set; } = string.Empty;
        public string SubCapitulo { get; set; } = string.Empty;
        public string CodigoDaf { get; set; } = string.Empty;
        public string Daf { get; set; } = string.Empty;
    }
}
