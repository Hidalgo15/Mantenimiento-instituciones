using Mantenimiento.Core.Application.DTOs.SubCapitulo;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class SubCapituloMapper
    {
        public static SubCapituloDto ToDto(SubCapitulo entity) => new()
        {
            Id = entity.Id,
            IdCapitulo = entity.IdCapitulo,
            CodigoSubCapitulo = entity.CodigoSubCapitulo,
            subcapitulo = entity.subcapitulo
        };

        public static SubCapitulo ToEntity(CrearSubCapituloDto dto) => new()
        {
            IdCapitulo = dto.IdCapitulo,
            CodigoSubCapitulo = dto.CodigoSubCapitulo,
            subcapitulo = dto.subcapitulo?.Trim() ?? string.Empty
        };

        public static SubCapitulo ToEntity(ActualizarSubCapituloDto dto) => new()
        {
            Id = dto.Id,
            IdCapitulo = dto.IdCapitulo,
            CodigoSubCapitulo = dto.CodigoSubCapitulo,
            subcapitulo = dto.subcapitulo?.Trim() ?? string.Empty
        };
    }
}
