using Mantenimiento.Core.Application.DTOs.Impedimentos;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class ImpedimentoMapper
    {
        public static ImpedimentoDto ToDto(Impedimento entity) => new()
        {
            Id = entity.Id,
            Nombres = entity.Nombres,
            CedulaRnc = entity.CedulaRnc,
            UsuarioAgrego = entity.UsuarioAgrego,
            Motivo = entity.Motivo,
            FechaAgrego = entity.FechaAgrego,
            Estado = entity.Estado
        };

        public static Impedimento ToEntity(CrearImpedimentoDto dto) => new()
        {
            Nombres = Normalizar(dto.Nombres),
            CedulaRnc = Normalizar(dto.CedulaRnc),
            UsuarioAgrego = Normalizar(dto.UsuarioAgrego),
            Motivo = Normalizar(dto.Motivo),
            Estado = dto.Estado
        };

        public static Impedimento ToEntity(ActualizarImpedimentoDto dto) => new()
        {
            Id = dto.Id,
            Nombres = Normalizar(dto.Nombres),
            CedulaRnc = Normalizar(dto.CedulaRnc),
            Motivo = Normalizar(dto.Motivo),
            Estado = dto.Estado
        };

        private static string? Normalizar(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
