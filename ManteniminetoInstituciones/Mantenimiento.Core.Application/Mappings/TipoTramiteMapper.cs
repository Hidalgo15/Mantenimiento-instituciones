using Mantenimiento.Core.Application.DTOs.TipoTramite;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class TipoTramiteMapper
    {
        public static TipoTramiteDto ToDto(TramiteContrato entity) => new()
        {
            Id = entity.Id,
            Tipo = entity.TipoTramite
        };

        public static TipoTramiteDto ToDto(TipoTramitePago entity) => new()
        {
            Id = entity.Id,
            Tipo = entity.Tipo?.Trim() ?? string.Empty
        };
    }
}
