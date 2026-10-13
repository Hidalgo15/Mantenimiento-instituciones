using Mantenimiento.Core.Application.DTOs.Fondos;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class FondoEspecialesMapper
    {
        public static FondoDto ToDto(FondosEspeciales entity) => new()
        {
            Id = entity.Id,
            Fondo = entity.Fondo,
            Descripcion = entity.Descripcion,
            Estructura = entity.Estructura,
            TipoTramiteContrato = new TramiteContrato
            {
                TipoTramite = entity.TipoTramite ?? string.Empty
            }
        };

        public static FondosEspeciales ToEntity(CrearFondoDto dto, string tipoTramiteNombre) => new()
        {
            Fondo = dto.Fondo.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            TipoTramite = tipoTramiteNombre
        };

        public static FondosEspeciales ToEntity(FondoDto dto, string tipoTramiteNombre) => new()
        {
            Id = dto.Id,
            Fondo = dto.Fondo.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            TipoTramite = tipoTramiteNombre
        };
    }
}
