namespace Mantenimiento.Core.Domain.Entities
{
    public class Subcapitulo
    {
        public int Id { get; set; }
        public int IdCapitulo { get; set; }
        public int CodigoSubcapitulo { get; set; }
        public string subcapitulo { get; set; } = string.Empty;
    }
}
