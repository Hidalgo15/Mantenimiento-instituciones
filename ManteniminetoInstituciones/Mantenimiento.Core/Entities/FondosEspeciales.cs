namespace Mantenimiento.Core.Domain.Entities
{
    public class FondosEspeciales
    {
        public int Id { get; set; }
        public string Fondo { get; set; } = string.Empty;
        public string? Descripcion { get; set; } = string.Empty;
        public string? Estructura { get; set; } = string.Empty;

        // Clave Foránea (columna escalar en la BD)
        public int? TipoTramiteContratoId { get; set; }

        // Propiedad de Navegación (Entidad Relacionada)
        public TipoTramiteContrato? TipoTramiteContrato { get; set; }
    }
}