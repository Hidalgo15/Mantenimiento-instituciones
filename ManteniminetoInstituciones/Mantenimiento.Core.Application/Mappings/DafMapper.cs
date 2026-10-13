using Mantenimiento.Core.Application.DTOs.Daf;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class DafMapper
    {
        public static DafDto ToDto(Daf entity) => new()
        {
            Id = entity.Id,
            IdSubCapitulo = entity.IdSubCapitulo,
            CodigoDaf = entity.CodigoDaf,
            Daf = entity.NombreDaf
        };

        public static Daf ToEntity(CrearDafDTO dto) => new()
        {
            IdSubCapitulo = dto.IdSubCapitulo,
            CodigoDaf = dto.CodigoDaf,
            NombreDaf = dto.Daf
        };

        public static Daf ToEntity(ActualizarDafDTO dto) => new()
        {
            Id = dto.Id,
            IdSubCapitulo = dto.IdSubCapitulo,
            CodigoDaf = dto.CodigoDaf,
            NombreDaf = dto.Daf
        };
    }
}
