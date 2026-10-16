using Mantenimiento.Core.Application.DTOs.Capitulo;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class CapituloMapper
    {
        public static CapituloDto ToDto(Capitulo entity) => new()
        {
            Id = entity.Id,
            CodigoCapitulo = entity.CodigoCapitulo,
            NombreCapitulo = entity.NombreCapitulo
        };

        public static Capitulo ToEntity(CrearCapituloDto dto) => new()
        {
            CodigoCapitulo = dto.CodigoCapitulo.Trim(),
            NombreCapitulo = dto.NombreCapitulo.Trim()
        };

        public static Capitulo ToEntity(ActualizarCapituloDto dto) => new()
        {
            Id = dto.Id,
            CodigoCapitulo = dto.CodigoCapitulo.Trim(),
            NombreCapitulo = dto.NombreCapitulo.Trim()
        };
    }
}
