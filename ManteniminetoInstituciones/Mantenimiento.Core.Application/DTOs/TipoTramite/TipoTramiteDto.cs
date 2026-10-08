

namespace Mantenimiento.Core.Application.DTOs.TipoTramite
{
    public record TipoTramiteDto
    {
        public int Id { get; set; }
        public string TipoTramite { get; set; } = string.Empty;
    }
}
