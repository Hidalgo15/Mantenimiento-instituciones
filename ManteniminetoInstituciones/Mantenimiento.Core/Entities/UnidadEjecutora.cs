namespace Mantenimiento.Core.Domain.Entities
{
    public class UnidadEjecutora
    {
        public int Id { get; set; }
        public int? IdPadre { get; set; }
        public int IdDaf { get; set; }
        public string? CodigoDaf { get; set; }
        public string? NombreDaf { get; set; }
        public string? CodigoUnidadEjecutora { get; set; }
        public string? NombreUnidadEjecutora { get; set; }
        public string? Rnc { get; set; }
        public bool Estado { get; set; }
        public int PortalCompra { get; set; }
        public DateTime? Creado { get; set; }
        public DateTime? Modificado { get; set; }
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
}
