
namespace Mantenimiento.Core.Domain.Entities
{
    public class FondosEspeciales
    {
        public int Id { get; set; }
        public string Fondo { get; set; } = string.Empty;
        public string? Descripcion { get; set; } = string.Empty;
        public string? Estructura { get; set; } = string.Empty;
        public TipoTramiteContrato? TipoTramiteContrato { get; set; } = new TipoTramiteContrato();
    }
}
