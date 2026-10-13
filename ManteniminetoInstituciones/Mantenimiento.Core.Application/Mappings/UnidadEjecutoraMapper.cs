using Mantenimiento.Core.Application.DTOs.UnidadEjecutora;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class UnidadEjecutoraMapper
    {
        public static UnidadEjecutoraDto ToDto(UnidadEjecutora entity) => new()
        {
            Id = entity.Id,
            IdPadre = entity.IdPadre,
            IdDaf = entity.IdDaf,
            IdCategoria = entity.IdCategoria,
            Estructura = entity.Estructura,
            DescripcionCategoria = entity.DescripcionCategoria,
            CodigoDaf = entity.CodigoDaf ?? string.Empty,
            NombreDaf = entity.NombreDaf ?? string.Empty,
            CodigoUnidadEjecutora = entity.CodigoUnidadEjecutora ?? string.Empty,
            NombreUnidadEjecutora = entity.NombreUnidadEjecutora ?? string.Empty,
            Rnc = entity.Rnc,
            Estado = entity.Estado,
            PortalCompra = entity.PortalCompra
        };

        public static UnidadEjecutora ToEntity(CrearUnidadEjecutoraDto dto) => new()
        {
            IdPadre = null,
            IdDaf = dto.IdDaf,
            IdCategoria = dto.IdCategoria,
            CodigoUnidadEjecutora = dto.CodigoUnidadEjecutora.Trim(),
            NombreUnidadEjecutora = dto.NombreUnidadEjecutora.Trim(),
            Rnc = dto.Rnc?.Trim(),
            Estado = dto.Estado,
            PortalCompra = dto.PortalCompra ? 1 : 0
        };

        public static UnidadEjecutora ToEntity(ActualizarUnidadEjecutoraDto dto) => new()
        {
            Id = dto.Id,
            IdPadre = dto.Id,
            IdDaf = dto.IdDaf,
            IdCategoria = dto.IdCategoria,
            Estructura = dto.Estructura,
            CodigoUnidadEjecutora = dto.CodigoUnidadEjecutora.Trim(),
            NombreUnidadEjecutora = dto.NombreUnidadEjecutora.Trim(),
            Rnc = dto.Rnc?.Trim(),
            Estado = dto.Estado,
            PortalCompra = dto.PortalCompra ? 1 : 0
        };
    }
}
