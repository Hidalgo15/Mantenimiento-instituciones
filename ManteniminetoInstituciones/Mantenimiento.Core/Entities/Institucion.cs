namespace Mantenimiento.Core.Domain.Entities
{
    public class Institucion
    {
        public int Id { get; set; }
        public int IdInstitucion { get; set; }
        public int Estatus { get; set; }
        public string Estructura { get; set; }
        public string Capitulo { get; set; }
        public string SubCapitulo { get; set; }
        public string Daf { get; set; }
        public string UnidadEjecutora { get; set; }
        public string Rnc { get; set; }
        public int PortalCompra { get; set; }
    }
}
