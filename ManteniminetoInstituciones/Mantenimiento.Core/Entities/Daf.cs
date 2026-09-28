namespace Mantenimiento.Core.Domain.Entities
{
    public class Daf
    {
        public int Id { get; set; }
        public int IdSubCapitulo { get; set; }
        public string CodigoDaf { get; set; } = string.Empty;
        public string NombreDaf { get; set; } = string.Empty;
    }
}
