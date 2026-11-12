using Mantenimiento.Core.Application.DTOs.ProveedoresInhabilitados;
using Mantenimiento.Core.Domain.Entities;

namespace Mantenimiento.Core.Application.Mappings
{
    public static class ProveedorInhabilitadoMapper
    {
        public static ProveedorInhabilitadoDto ToDto(ProveedorInhabilitado entity) => new()
        {
            Id = entity.Id,
            Rpe = entity.Rpe,
            RazonSocial = entity.RazonSocial,
            Genero = entity.Genero,
            NumeroDocumento = entity.NumeroDocumento,
            TipoDocumento = entity.TipoDocumento,
            TipoPersona = entity.TipoPersona,
            FechaRegistro = entity.FechaRegistro,
            FechaUltimaActualizacion = entity.FechaUltimaActualizacion,
            EstadoRpe = entity.EstadoRpe,
            MotivoInhabilitacion = entity.MotivoInhabilitacion
        };

        public static ProveedorInhabilitado ToEntity(CrearProveedorInhabilitadoDto dto) => new()
        {
            Rpe = Normalizar(dto.Rpe),
            RazonSocial = Normalizar(dto.RazonSocial),
            Genero = Normalizar(dto.Genero),
            NumeroDocumento = Normalizar(dto.NumeroDocumento),
            TipoDocumento = Normalizar(dto.TipoDocumento),
            TipoPersona = Normalizar(dto.TipoPersona),
            EstadoRpe = Normalizar(dto.EstadoRpe),
            MotivoInhabilitacion = Normalizar(dto.MotivoInhabilitacion)
        };

        public static ProveedorInhabilitado ToEntity(ActualizarProveedorInhabilitadoDto dto) => new()
        {
            Id = dto.Id,
            Rpe = Normalizar(dto.Rpe),
            RazonSocial = Normalizar(dto.RazonSocial),
            Genero = Normalizar(dto.Genero),
            NumeroDocumento = Normalizar(dto.NumeroDocumento),
            TipoDocumento = Normalizar(dto.TipoDocumento),
            TipoPersona = Normalizar(dto.TipoPersona),
            EstadoRpe = Normalizar(dto.EstadoRpe),
            MotivoInhabilitacion = Normalizar(dto.MotivoInhabilitacion)
        };

        private static string? Normalizar(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
